using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    [DataContract]
    public class Book : BookResult
    {
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string Title { get; set; }

        private string _fullTitle = null;
        [DataMember]
        public new string FullTitle
        {
            get { return _fullTitle ?? $"{Author}: {Title}"; }
            set { _fullTitle = value; }
        }

        [DataMember]
        public string Urn { get; set; }
        [DataMember]
        public string MekId { get; set; }
        [DataMember]
        public Uri Source { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public string Period { get; set; }
        [DataMember]
        public string Contents { get; set; }
        [DataMember]
        public string Prologue { get; set; }
        [DataMember]
        public string Epilogue { get; set; }
        [DataMember]
        public string Summary { get; set; }
        [DataMember]
        public string Publisher { get; set; }
        [DataMember]
        public string PublishYear { get; set; }
        [DataMember]
        public string PublishPlace { get; set; }
        [DataMember]
        public List<Contributor> Creators { get; set; }
        [DataMember]
        public List<Contributor> Contributors { get; set; }
        [DataMember]
        public List<string> Topics { get; set; }
        [DataMember]
        public List<string> Tags { get; set; }
        [DataMember]
        public List<string> KeyWords { get; set; }

        [DataMember]
        public List<BookResult> Related { get; set; }


        public void Merge(Book from)
        {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
