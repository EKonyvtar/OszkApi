using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    //TODO: fix this and move to api model with routes:
    // https://docs.asp.net/en/latest/fundamentals/routing.html#url-generation-reference

    [DataContract]
    public class BookMetadata
    {
        private static string URL_MEK_ENDPOINT = "http://mek.oszk.hu/{0}";

        public string Id { get; set; }

        [DataMember]
        public string Uri { get { return $"/audiobooks/{Id}"; } }


        private string _mekUrl = null;
        [DataMember]
        public string MekUrl
        {
            get { return _mekUrl ?? string.Format(URL_MEK_ENDPOINT, CatalogResolver.Resolve(Id).UrlId); }
            set { _mekUrl = value; }
        }

    }
}
