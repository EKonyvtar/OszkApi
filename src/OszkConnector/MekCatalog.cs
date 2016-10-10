using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OszkConnector
{
    public class MekCatalog
    {
        private const string MEK_ENDPOINT_URL = "http://mek.oszk.hu";

        private static KeyValuePair<string, string> data(string field, string value = null)
        {
            return new KeyValuePair<string, string>(field, value);
        }

        public static List<KeyValuePair<string, string>> SearchFilter(int fieldIndex, string field, string value = "", string operation = "and", byte order = 1)
        {
            //eg: "s1=dc_format+format_name&m1=MP3&muv1=and"
            var filter = new List<KeyValuePair<string, string>>();
            filter.Add(data($"s{order}", field));
            //filter.Add(data($"sind{order}", fieldIndex.ToString()));
            filter.Add(data($"m{order}", value));

            if (!string.IsNullOrWhiteSpace(operation))
            {
                filter.Add(data($"muv{order}", operation));

                //if (order > 1)
                //    filter.Add(data($"muv{order - 1}index", "0"));
            }
            return filter;
        }

        public static async Task<IQueryable<Book>> SearchAsync(string query = "", string format = "")
        {
            var uri = new Uri($"{MEK_ENDPOINT_URL}/katalog/kataluj.php3");
            var filter = new List<KeyValuePair<string, string>>();

            filter.Add(data("szerint", "cimsz"));

            //TODO: Resolve field Indexes
            filter.AddRange(SearchFilter(0, "dc_format+format_name", format, "and", 1));
            filter.AddRange(SearchFilter(7, "dc_creator_o+FamilyGivenName", query, "or", 2));
            filter.AddRange(SearchFilter(16, "dc_title+subtitle", "", "or", 3));
            filter.AddRange(SearchFilter(13, "dc_subject+keyword", "", "or", 4));
            filter.AddRange(SearchFilter(18, "dc_subject+keyword", "", "", 5));

            filter.Add(data("ekezet", "ektelen"));

            filter.Add(data("x", "31"));
            filter.Add(data("y", "8"));

            //filter.Add(data("subid", ""));
            filter.Add(data("sind1", "0"));
            filter.Add(data("sind2", "7"));
            filter.Add(data("sind3", "16"));
            filter.Add(data("sind4", "13"));
            filter.Add(data("sind5", "18"));

            filter.Add(data("muv1index", "0"));
            filter.Add(data("muv2index", "0"));
            filter.Add(data("muv3index", "0"));
            filter.Add(data("muv4index", "0"));
            
            filter.Add(data("subid_check",""));

            filter.Add(data("ekezet_check", ""));
            

            
            var content = new FormUrlEncodedContent(filter);
            var response = await new HttpClient().PostAsync(uri, content);
            var html = MekConvert.ToUtf8(await response.Content.ReadAsByteArrayAsync());
            return MekFactory.CreateBookFromResultPage(html);
        }

    }
}
