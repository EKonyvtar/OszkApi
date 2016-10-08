using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class MekFactory
    {
        public static List<string> CreateStringsFromIndexNode(HtmlNodeCollection nodeCollection, string nodeName = null)
        {
            var strings = new List<string>();
            foreach (var node in nodeCollection)
                if (nodeName != null)
                    strings.Add(node.ChildNodes[nodeName]?.InnerText);
                else
                    strings.Add(node.InnerText);

            return strings;
        }
        public static List<Contributor> CreateContributorsFromIndexNode(HtmlNodeCollection nodeCollection)
        {
            var contributors = new List<Contributor>();
            foreach (var node in nodeCollection)
                try
                {
                    contributors.Add(new Contributor()
                    {
                        FamilyName = node.ChildNodes["familyname"]?.InnerText,
                        GivenName = node.ChildNodes["givenname"]?.InnerText,
                        Role = node.ChildNodes["role"]?.InnerText
                    });
                }
                catch
                {
                    //TODO: log
                }

            return contributors;
        }

        public static List<BookResult> CreateBooksFromIndexNode(HtmlNodeCollection nodeCollection)
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
                        FullTitle = MekConverter.ClearFullTitle(title),
                        Title = MekConverter.ToTitle(title),
                        Author = MekConverter.ToAuthor(title),
                        UrlId = CatalogResolver.Resolve(url)?.UrlId
                    });
                }
                catch
                {
                    //TODO: log
                }
            return books;
        }
    }
}
