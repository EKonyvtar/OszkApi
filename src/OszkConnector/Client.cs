using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using OszkConnector.Models;
using HtmlAgilityPack;
using System.Linq;

namespace OszkConnector
{
    public class Client
    {
        public const string MEK_ENDPOINT_URL = "https://mek.oszk.hu";

        private static byte[] FetchUrlContent(Uri uri)
        {
            var baseAddress = new Uri(MEK_ENDPOINT_URL);
            var cookieContainer = new System.Net.CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                //client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0(Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36(KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36 Edg/124.0.0.0");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");
                cookieContainer.Add(baseAddress, new System.Net.Cookie("SID", "5da0ab8750"));
                
                // Make sure to use `GetAwaiter().GetResult()` to avoid deadlocks.
                HttpResponseMessage response = client.GetAsync(uri).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                byte[] content = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                return content;
            }
        }

        private async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            var baseAddress = new Uri(MEK_ENDPOINT_URL);
            var cookieContainer = new System.Net.CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookieContainer.Add(baseAddress, new System.Net.Cookie("nagykoru", "igen"));
                return await client.GetAsync(uri);
            }
        }

        public async Task<IQueryable<Book>> FindAudioBook(string query = "")
        {
            var uri = new Uri($"{MEK_ENDPOINT_URL}/katalog/kataluj.php3");
            var postData = $"szerint=cimsz&s1=dc_format+format_name&m1=MP3&muv1=and&s2=dc_creator_o+FamilyGivenName&m2={query}&muv2=or&s3=dc_title+subtitle&m3=&muv3=or&s4=dc_subject+keyword&m4=&muv4=or&s5=dc_subject+keyword&m5=&subid=&x=31&y=8&sind1=0&sind2=7&sind3=16&sind4=13&sind5=18&muv1index=0&muv2index=0&muv3index=0&muv4index=0&subid_check=&ekezet_check=&ekezet=ektelen";
            var content = new FormUrlEncodedContent(MekConvert.ToKeyValuePair(postData));

            var response = await new HttpClient().PostAsync(uri, content);
            var html = MekConvert.ToUtf8(await response.Content.ReadAsByteArrayAsync());
            var books = MekFactory.CreateBookListFromResultPage(html);
            return books;
        }

        public async Task<IQueryable<Book>> FindBook(string query = "")
        {
            return await MekCatalog.SearchAsync(query, "MP3");
        }

        public async Task<Book> GetBook(string catalogId)
        {
            var urlId = CatalogResolver.Resolve(catalogId).UrlId;
            var uri = new Uri($"{MEK_ENDPOINT_URL}/{urlId}/index.xml");
            //var response = await GetAsync(uri);
            var response = FetchUrlContent(uri);
            var html = MekConvert.ToIso88592(response);
            return MekFactory.CreateBookFromIndex(html);
        }

        public async Task<AudioBook> GetAudioBook(string catalogId)
        {
            var urlId = CatalogResolver.Resolve(catalogId).UrlId;
            var url = $"{MEK_ENDPOINT_URL}/{urlId}/mp3/";
            var page = $"{url}index.html";
            //var response = await GetAsync(new Uri(page));
            var response = FetchUrlContent(new Uri(page));
            //var html = MekConvert.ToUtf8(await response.Content.ReadAsByteArrayAsync());
            var html = MekConvert.ToUtf8(response);
            var trackBook = MekFactory.CreateAudioBookFromMP3Page(url, html);
            var audioBook = await GetBook(catalogId);
            trackBook.Copy(audioBook);

            return trackBook;
        }
    }
}
