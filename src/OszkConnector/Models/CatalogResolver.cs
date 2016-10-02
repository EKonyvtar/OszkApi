using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class CatalogResolver
    {
        private const string REGEX_PROTOCOL = "^https?://";
        private const string REGEX_URL_ID = "(\\d+[\\\\/]\\d+)";

        public static CatalogResolver Resolve(string id)
        {
            if (id != null)
                return new CatalogResolver(id);
            return null;
        }

        public string UrlId { get; private set; }
        public string FullUrl { get; private set; }


        public CatalogResolver(string url)
        {
            Match matches = null;

            if (Regex.IsMatch(url, REGEX_PROTOCOL))
                FullUrl = url;

            matches = Regex.Match(url, REGEX_URL_ID);
            if (matches != null)
                UrlId = matches.Groups[1].Value;
        }
    }
}
