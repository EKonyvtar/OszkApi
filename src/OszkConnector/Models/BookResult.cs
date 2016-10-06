using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class BookResult
    {
        private static string URL_MEK_THUMBNAIL = "http://mek.oszk.hu/{0}/borito.jpg";
        private static string URL_MEK_ENDPOINT = "http://mek.oszk.hu/{0}";

        public string Author { get; set; }
        public string Title { get; set; }
        public string UrlId { get; set; }

        private string _url = null;
        public string Url
        {
            get { return _url ?? string.Format(URL_MEK_ENDPOINT, UrlId); }
            set { _url = value; }
        }

        private Uri _ThumbnailUrl = null;
        public Uri ThumbnailUrl
        {
            get
            {
                return _ThumbnailUrl ??
                    new Uri(string.Format(URL_MEK_THUMBNAIL, UrlId));
            }
            set
            {
                _ThumbnailUrl = value;
            }
        }


        private string _fullTitle = null;
        public string FullTitle
        {
            get { return _fullTitle ?? $"{Author} - {Title}"; }
            set { _fullTitle = value; }
        }

        public override string ToString()
        {
            return $"[{UrlId}] {FullTitle}";
        }
    }
}
