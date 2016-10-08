using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    [DataContract]
    public class BookResult
    {
        private static string URL_MEK_THUMBNAIL = "http://mek.oszk.hu/{0}/borito.jpg";
        private static string URL_MEK_ENDPOINT = "http://mek.oszk.hu/{0}";

        [DataMember]
        public string Id { get; set; }

        private string _urlId = null;

        public string UrlId
        {
            get { return _urlId ?? $"{Id.Substring(0, 3)}00/{Id}"; }
            set { _urlId = value; }
        }

        private string _url = null;


        [DataMember]
        public string FullTitle { get; set; }

        [DataMember]
        public string Url
        {
            get { return _url ?? string.Format(URL_MEK_ENDPOINT, UrlId); }
            set { _url = value; }
        }

        private Uri _ThumbnailUrl = null;
        [DataMember]
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

        public override string ToString()
        {
            return $"[{Id}] {FullTitle}";
        }
    }
}
