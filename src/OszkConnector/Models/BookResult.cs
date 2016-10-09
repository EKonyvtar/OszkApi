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

        private static string _id = null;
        [DataMember]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                Metadata.Id = value;
            }
        }


        //TODO: eliminate UrlId
        private string _urlId = null;

        public string UrlId
        {
            get { return _urlId ?? $"{Id.Substring(0, 3)}00/{Id}"; }
            set { _urlId = value; }
        }

        [DataMember]
        public string FullTitle { get; set; }


        private Uri _ThumbnailUrl = null;
        [DataMember]
        public Uri ThumbnailUrl
        {
            get
            {
                return _ThumbnailUrl ?? new Uri(string.Format(URL_MEK_THUMBNAIL, UrlId));
            }
            set { _ThumbnailUrl = value; }
        }

        [DataMember(Name = "__metadata")]
        public BookMetadata Metadata { get; set; }

        public BookResult()
        {
            Metadata = new BookMetadata();
        }
        public override string ToString()
        {
            return $"[{Id}] {FullTitle}";
        }
    }
}
