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
        private const string REGEX_URL_ID = "(\\d+[\\\\/](\\d+))";
        private const string REGEX_MEK_ID = @"(MEK-(\d+))";
        private const string REGEX_NUM_ID = @"(\d+)";

        public static CatalogResolver Resolve(string id)
        {
            if (id != null)
                return new CatalogResolver(id);
            return null;
        }
        public string Id { get; set; }
        public int IntId { get { return Int32.Parse(Id); } }
        public string MekId { get; set; }
        public string UrlId { get; private set; }
        public string FullUrl { get; private set; }


        public CatalogResolver(string catalogReference)
        {
            Match matches = null;
            if (catalogReference == null) return;
            catalogReference = catalogReference.Trim();

            if (Regex.IsMatch(catalogReference, REGEX_PROTOCOL))
                FullUrl = catalogReference;

            matches = Regex.Match(catalogReference, REGEX_URL_ID);
            if (matches != null)
            {
                UrlId = matches.Groups[1].Value;
                Id = matches.Groups[2].Value;
            }

            matches = Regex.Match(catalogReference, REGEX_MEK_ID);
            if (matches != null)
            {
                MekId = matches.Groups[1].Value;
                Id = matches.Groups[2].Value;
            }

            MekId = MekId ?? $"MEK-{Id}";
            UrlId = UrlId ?? $"{Id.Substring(0, 3)}00/{Id}";
        }
    }
}
