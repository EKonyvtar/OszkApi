using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class MekFactory
    {
        public static string StringFromNode(HtmlNodeCollection nodeCollection, string nodeName = null)
        {
            return StringListFromNode(nodeCollection, nodeName)?.FirstOrDefault();
        }
        public static List<string> StringListFromNode(HtmlNodeCollection nodeCollection, string nodeName = null)
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
        public static List<Contributor> ContributorsFromNode(HtmlNodeCollection nodeCollection)
        {
            var contributors = new List<Contributor>();
            foreach (var node in nodeCollection)
                try
                {
                    contributors.Add(new Contributor()
                    {
                        FamilyName = MekConvert.ClearText(node.ChildNodes["familyname"]?.InnerText),
                        GivenName = MekConvert.ClearText(node.ChildNodes["givenname"]?.InnerText),
                        Role = MekConvert.ClearText(node.ChildNodes["role"]?.InnerText)
                    });
                }
                catch
                {
                    //TODO: log
                }

            return contributors;
        }

        public static List<Book> BooksFromNode(HtmlNodeCollection nodeCollection)
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
    }
}
