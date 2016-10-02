using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using OszkConnector.Models;
using HtmlAgilityPack;


namespace OszkConnector
{
    public class Client
    {
        private const string MEK_ENDPOINT_URL = "http://mek.oszk.hu";

        public async Task<IEnumerable<Book>> FindAudioBook(string query = "")
        {
            var uri = new Uri($"{MEK_ENDPOINT_URL}/katalog/kataluj.php3");
            var postData = $"szerint=cimsz&s1=dc_format+format_name&m1=MP3&muv1=and&s2=dc_creator_o+FamilyGivenName&m2={query}&muv2=or&s3=dc_title+subtitle&m3=&muv3=or&s4=dc_subject+keyword&m4=&muv4=or&s5=dc_subject+keyword&m5=&subid=&x=31&y=8&sind1=0&sind2=7&sind3=16&sind4=13&sind5=18&muv1index=0&muv2index=0&muv3index=0&muv4index=0&subid_check=&ekezet_check=&ekezet=ektelen";
            var content = new FormUrlEncodedContent(MekConverter.ToKeyValuePair(postData));

            var response = await new HttpClient().PostAsync(uri, content);
            var html = MekConverter.ToUtf8(await response.Content.ReadAsByteArrayAsync());
            var books = Client.ParseMekBookHtml(html);
            return books;
        }

        private static IEnumerable<Book> ParseMekBookHtml(string html)
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
    }
}
