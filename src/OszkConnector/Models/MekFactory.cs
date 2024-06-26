﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class MekFactory
    {
        private static string StringFromNode(HtmlNodeCollection nodeCollection, string nodeName = null)
        {
            return StringListFromNode(nodeCollection, nodeName)?.FirstOrDefault();
        }
        private static List<string> StringListFromNode(HtmlNodeCollection nodeCollection, string nodeName = null)
        {
            if (nodeCollection == null)
                return null;

            var strings = new List<string>();
            foreach (var node in nodeCollection)
            {
                string text = (nodeName != null) ?
                    node.ChildNodes[nodeName]?.InnerText :
                    node.InnerText;

                if (!string.IsNullOrWhiteSpace(text))
                    strings.Add(MekConvert.ClearText(text));
            }
            return strings;
        }
        private static List<Contributor> ContributorsFromNode(HtmlNodeCollection nodeCollection)
        {
            if (nodeCollection == null) return null;

            var contributors = new List<Contributor>();
            foreach (var node in nodeCollection)
                try
                {
                    contributors.Add(new Contributor()
                    {
                        FamilyName = MekConvert.ClearText(node.ChildNodes["familyname"]?.InnerText),
                        GivenName = MekConvert.ClearText(node.ChildNodes["givenname"]?.InnerText),
                        Role = MekConvert.ClearText(node.ChildNodes["role"]?.InnerText),
                        IsFamilyFirst = MekConvert.ClearText(node.ChildNodes["invert"].InnerText) == "nem"
                    });
                }
                catch
                {
                    //TODO: log
                }

            return contributors;
        }

        private static List<Book> BooksFromNode(HtmlNodeCollection nodeCollection)
        {
            /*
            <dc_relation>
                <MEK>
                    <MEK_name>Gárdonyi Géza: Bojgás az világba</MEK_name>
                    <MEK_URL>http://mek.oszk.hu/05700/05712/</MEK_URL>
                </MEK>
            </dc_relation>
            */
            var books = new List<Book>();
            foreach (var node in nodeCollection)
                try
                {
                    var title = node.ChildNodes["mek"]?.ChildNodes["mek_name"]?.InnerText;
                    var url = node.ChildNodes["mek"]?.ChildNodes["mek_url"]?.InnerText;
                    var catalog = CatalogResolver.Resolve(url);
                    books.Add(new Book()
                    {
                        FullTitle = MekConvert.ClearText(title),
                        Id = catalog?.Id,
                        UrlId = catalog?.UrlId
                    });
                }
                catch
                {
                    //TODO: log
                }
            return books;
        }

        public static Book CreateBookFromContentsPage(string pageContent)
        {
            var book = new Book();
            var document = new HtmlDocument();
            document.Load(new StringReader(pageContent));
            var root = document.DocumentNode;

            book.Contents = MekConvert.ClearText(root.SelectNodes("//tartalom").FirstOrDefault()?.InnerText);
            book.Prologue = root.SelectNodes("//eloszo").FirstOrDefault()?.InnerText;
            book.Epilogue = root.SelectNodes("//utoszo").FirstOrDefault()?.InnerText;
            book.Summary = root.SelectNodes("//ismerteto").FirstOrDefault()?.InnerText;
            return book;
        }

        public static Book CreateBookFromIndex(string content)
        {
            var book = new Book();
            var html = new HtmlDocument();
            html.Load(new StringReader(content));
            var doc = html.DocumentNode;

            var url = StringFromNode(doc.SelectNodes("//mek2/dc_identifier/url"));
            var catalog = CatalogResolver.Resolve(url);

            book.Id = catalog?.Id;
            book.UrlId = catalog?.UrlId;
            book.Metadata.MekUrl = url;
            book.MekId = StringFromNode(doc.SelectNodes("//mek2/dc_identifier/mekid"));
            book.Urn = StringFromNode(doc.SelectNodes("//mek2/dc_identifier/urn"));

            book.Title = MekConvert.ClearText(StringFromNode(doc.SelectNodes("//mek2/dc_title/main")));

            Uri source = null;
            Uri.TryCreate(StringFromNode(doc.SelectNodes("//mek2/dc_source/act_url")), UriKind.RelativeOrAbsolute, out source);
            book.Source = source;

            book.Type = StringListFromNode(doc.SelectNodes("//mek2/dc_type"));

            book.Topics = new List<string>();
            book.Topics.Add(StringFromNode(doc.SelectNodes("//mek2/dc_subject/topicgroup/broadtopic")));
            book.Topics.Add(StringFromNode(doc.SelectNodes("//mek2/dc_subject/topicgroup/topic")));
            book.Topics.Add(StringFromNode(doc.SelectNodes("//mek2/dc_subject/topicgroup/subtopic")));

            book.KeyWords = StringListFromNode(doc.SelectNodes("//mek2/dc_subject/keyword"));

            book.Period = StringFromNode(doc.SelectNodes("//mek2/dc_subject/period"));
            book.Language = StringFromNode(doc.SelectNodes("//mek2/dc_language/lang"));

            book.Creators = ContributorsFromNode(doc.SelectNodes("//mek2/dc_creator"));
            book.Author = book.Creators?.First()?.ToString();
            book.Contributors = ContributorsFromNode(doc.SelectNodes("//mek2/dc_contributor"));

            var publisher = doc.SelectNodes("//mek2/dc_publisher")?.First();
            if (publisher != null)
            {
                book.Publisher = MekConvert.ClearText(publisher.ChildNodes["pub_name"]?.InnerText);
                book.PublishPlace = MekConvert.ClearText(publisher.ChildNodes["place"]?.InnerText);
                book.PublishYear = MekConvert.ClearText(publisher.ChildNodes["publishYear"]?.InnerText);
            }

            book.Related = BooksFromNode(doc.SelectNodes("//mek2/dc_relation"));

            return book;
        }

        public static AudioBookTrack CreateAudioBookTrack(string url, string trackLine)
        {
            var track = new AudioBookTrack();

            // Filename and title match
            var basicMatch = new Regex(@"((\d+)?.+\.(mp3))?\s?-\s?(.+)").Match(trackLine);
            //Eg: "01_bojgas.mp3 - Itt kezdődik (10:53 min. 7,8 Mbyte)"
            //     2     1    3           4
            if (basicMatch.Success && !string.IsNullOrEmpty(basicMatch.Groups[1].Value))
            {
                track.FileName = MekConvert.Trim(basicMatch.Groups[1].Value);
                track.FileUrl = new Uri($"{url}{track.FileName}");
                track.Title = MekConvert.ClearText(basicMatch.Groups[4].Value);
            } else
            {
                throw new FormatException($"{trackLine} could not be parsed");
            }

            //Title strip
            var titleMatch = new Regex(@"(.+)[(](.*)[)]").Match(track.Title);
            //Eg: Itt kezdődik (10:53 min. 7,8 Mbyte)"
            //      1                2
            if (titleMatch.Success && !string.IsNullOrEmpty(titleMatch.Groups[1].Value))
            {
                track.Title = MekConvert.ClearText(titleMatch.Groups[1].Value);
                track.MetaData = titleMatch.Groups[2].Value.Trim();
            }

            //Size strip
            var sizeMatch = new Regex(@"((\d+[.,]?\d+)\s*([MmKk](ega|ilo)?[Bb](yte|ajt|ájt)?))").
                Match(track.MetaData??"");
            //Eg: 7,8 Mbyte
            //    2     3
            if (sizeMatch.Success && !string.IsNullOrEmpty(sizeMatch.Groups[1].Value))
                track.Size = sizeMatch.Groups[1].Value.Trim();

            //Length strip
            var lengthMatch = new Regex(@"(((\d+):)?(\d+):+(\d+))\s*(min|perc)").
                Match(track.MetaData??"");
            //Eg: 0:10:53 perc
            //    3  4  5
            if (lengthMatch.Success && !string.IsNullOrEmpty(lengthMatch.Groups[1].Value)) {
                track.Length = lengthMatch.Groups[1].Value.Trim();
                var timeSpan = new TimeSpan();
                try
                {
                    //TimeSpan.TryParseExact(track.Length, "t", new CultureInfo("hu-HU"), out timeSpan);
                    timeSpan = new TimeSpan(
                        Convert.ToInt32("0" + lengthMatch.Groups[3].Value),
                        Convert.ToInt32("0" + lengthMatch.Groups[4].Value),
                        Convert.ToInt32("0" + lengthMatch.Groups[5].Value)
                    );
                }
                catch (Exception e)
                {
                    //TODO: log properly parsing error
                    throw e;
                }
                finally {
                    track.LengthTotalSeconds = (int)timeSpan.TotalSeconds;
                }
            }
            return track;
        }
        public static AudioBook CreateAudioBookFromMP3Page(string url, string html)
        {
            var audioBook = new AudioBook();
            audioBook.Tracks = new List<AudioBookTrack>();

            var document = new HtmlDocument();
            document.Load(new StringReader(html));

            var catalog = CatalogResolver.Resolve(url);
            audioBook.Id = catalog?.Id;

            try
            {
                foreach (var li in document.DocumentNode.SelectNodes("//li"))
                    try
                    {
                        audioBook.Tracks.Add(CreateAudioBookTrack(url, li.InnerText));
                    }
                    catch (FormatException fe) { } //TODO: log
                    catch (Exception e)
                    {
                        //TODO: log parse error
                        throw e;
                    }
            } catch(Exception e)
            {
                // /25185.mp3
                var singleFile = $"{catalog.Id}.mp3";
                var fileUrl = new Uri(catalog.FullUrl.Replace("mp3/", singleFile));

                //var response = await GetAsync(new Uri(fileUrl));
                audioBook.Tracks.Add(new AudioBookTrack()
                {
                    FileName = singleFile,
                    FileUrl = fileUrl
                });
                return audioBook;
            }

            return audioBook;
        }

        public static IQueryable<Book> CreateBookListFromResultPage(string pageContent)
        {
            var books = new List<Book>();

            var document = new HtmlDocument();
            document.Load(new StringReader(pageContent));
            var docNode = document.DocumentNode;
            foreach (var f in docNode.SelectNodes("//a[contains(@href,'Javascript')]"))
            {
                try
                {
                    var url = f.ParentNode.ParentNode.SelectSingleNode("span").FirstChild.InnerText;
                    var catalog = CatalogResolver.Resolve(url);
                    books.Add(new Book()
                    {
                        FullTitle = MekConvert.ClearText(f.InnerText),
                        Id = catalog?.Id,
                        UrlId = catalog?.UrlId
                    });
                }
                catch (Exception ex)
                {
                    //TODO: log error
                }
            }
            return books.AsQueryable();
        }
    }
}
