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
                    strings.Add(MekConvert.TrimAll(text));
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
                        FamilyName = MekConvert.TrimAll(node.ChildNodes["familyname"]?.InnerText),
                        GivenName = MekConvert.TrimAll(node.ChildNodes["givenname"]?.InnerText),
                        Role = MekConvert.TrimAll(node.ChildNodes["role"]?.InnerText)
                    });
                }
                catch
                {
                    //TODO: log
                }

            return contributors;
        }

        public static List<BookResult> BooksFromNode(HtmlNodeCollection nodeCollection)
        {
            /*
            <dc_relation>
                <MEK>
                    <MEK_name>Gárdonyi Géza: Bojgás az világba</MEK_name>
                    <MEK_URL>http://mek.oszk.hu/05700/05712/</MEK_URL>
                </MEK>
            </dc_relation>
            */
            var books = new List<BookResult>();
            foreach (var node in nodeCollection)
                try
                {
                    var title = node.ChildNodes["mek"]?.ChildNodes["mek_name"]?.InnerText;
                    var url = node.ChildNodes["mek"]?.ChildNodes["mek_url"]?.InnerText;

                    books.Add(new BookResult()
                    {
                        FullTitle = MekConvert.ClearFullTitle(title),
                        Title = MekConvert.ToTitle(title),
                        Author = MekConvert.ToAuthor(title),
                        UrlId = CatalogResolver.Resolve(url)?.UrlId
                    });
                }
                catch
                {
                    //TODO: log
                }
            return books;
        }

        public static Book CreateBookFromIndex(string content)
        {
            var book = new Book();
            var html = new HtmlDocument();
            html.Load(new StringReader(content));
            var doc = html.DocumentNode;

            book.Url = StringFromNode(doc.SelectNodes("//mek2/dc_identifier/url"));
            book.UrlId = CatalogResolver.Resolve(book.Url).UrlId;
            book.MekId = StringFromNode(doc.SelectNodes("//mek2/dc_identifier/mekid"));
            book.Urn = StringFromNode(doc.SelectNodes("//mek2/dc_identifier/urn"));

            book.Title = MekConvert.ClearFullTitle(StringFromNode(doc.SelectNodes("//mek2/dc_title/main")));

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
                book.Publisher = MekConvert.TrimAll(publisher.ChildNodes["pub_name"]?.InnerText);
                book.PublishPlace = MekConvert.TrimAll(publisher.ChildNodes["place"]?.InnerText);
                book.PublishYear = MekConvert.TrimAll(publisher.ChildNodes["publishYear"]?.InnerText);
            }

            book.Related = BooksFromNode(doc.SelectNodes("//mek2/dc_relation"));

            return book;
        }
    }
}
