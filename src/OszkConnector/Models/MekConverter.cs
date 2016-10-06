using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class MekConverter
    {
        public const string STR_AUDIOBOOK = "[Hangoskönyv]";
        public const string STR_NBSP = "&nbsp;";

        private static Regex REGEX_TITLE = new Regex($"^(.*){STR_NBSP}(.*)$");

        public static string ToUtf8(byte[] byteArray)
        {
            //TODO: Implement legacy conversion from ISO-8859-2
            var iso = Encoding.GetEncoding("ISO-8859-1");
            var utf8 = Encoding.UTF8;
            var utf8Bytes = Encoding.Convert(iso, utf8, byteArray);

            return utf8.GetString(utf8Bytes);
        }

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePair(string postData)
        {
            var cc = new List<KeyValuePair<string, string>>();
            foreach (var field in postData.Split('&'))
            {
                var key = field.Split('=')[0];
                var value = field.Split('=')[1];
                cc.Add(new KeyValuePair<string, string>(key, value?.Replace('+', ' ')));
            }
            return cc;
        }

        public static string ToFullTitle(string text)
        {
            return text.
                Replace(STR_AUDIOBOOK, "").
                Replace(STR_NBSP, " ").
                Trim();
        }

        public static string ToAuthor(string text)
        {
            if (text.Contains(":"))
                return ToFullTitle(text).Split(':')[0]?.Trim();
            return null;
        }

        public static string ToTitle(string text)
        {
            if (text.Contains(":"))
                return ToFullTitle(text).Split(':')[1]?.Trim();
            return text;

        }

        public static AudioBookTrack ConvertFrom(string text)
        {
            //Eg: "01_bojgas.mp3 - Itt kezdődik (10:53 min. 7,8 Mbyte)"
            //     2     1    3           4                   5

            var converterRegex = new Regex(@"((\d+)?.+\.(mp3))?\s?-\s?(.+)\s?(\(.+\))");
            var match = converterRegex.Match(text);

            if (match != null)
                return new AudioBookTrack()
                {
                    Track = Convert.ToInt32(match.Groups[2].Value),
                    FileName = match.Groups[1].Value,
                    Title = match.Groups[4].Value
                };


            return null;
        }

        public static IEnumerable<AudioBookTrack> ParseMekMP3Page(string html)
        {
            var tracks = new List<AudioBookTrack>();

            var document = new HtmlDocument();
            document.Load(new StringReader(html));
            foreach (var li in document.DocumentNode.SelectNodes("//li"))
                try
                {
                    tracks.Add(MekConverter.ConvertFrom(li.InnerText));
                }
                catch
                {
                    //TODO: log parse error
                }
            return tracks;
        }

        public static IEnumerable<Book> ParseMekBookResultPage(string html)
        {
            var books = new List<Book>();

            var document = new HtmlDocument();
            document.Load(new StringReader(html));
            var docNode = document.DocumentNode;
            foreach (var f in docNode.SelectNodes("//a[contains(@href,'Javascript')]"))
            {
                try
                {
                    var url = f.ParentNode.ParentNode.SelectSingleNode("span").FirstChild.InnerText;
                    books.Add(new Book()
                    {
                        FullTitle = MekConverter.ToFullTitle(f.InnerText),
                        Title = MekConverter.ToTitle(f.InnerText),
                        Author = MekConverter.ToAuthor(f.InnerText),
                        UrlId = CatalogResolver.Resolve(url)?.UrlId
                    });
                }
                catch (Exception ex)
                {
                    //TODO: log error
                }
            }
            return books;
        }

        public static Book ParseMekBookPage(string html)
        {
            var book = new Book();
            var document = new HtmlDocument();
            document.Load(new StringReader(html));
            var docNode = document.DocumentNode;
            foreach (var f in docNode.SelectNodes("//a[contains(@href,'Javascript')]"))
            {
                try
                {
                    var url = f.ParentNode.ParentNode.SelectSingleNode("span").FirstChild.InnerText;
                }
                catch (Exception ex)
                {
                    //TODO: log error
                }
            }
            return book;
        }
    }
}
