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
        [DataMember(EmitDefaultValue = false)]
        public string Author { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Title { get; set; }

        private string _fullTitle = null;
        [DataMember(EmitDefaultValue = false)]
        public new string FullTitle
        {
            get { return _fullTitle ?? $"{Author}: {Title}"; }
            set { _fullTitle = value; }
        }

        [DataMember(EmitDefaultValue = false)]
        public string Urn { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string MekId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Uri Source { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Language { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Period { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Contents { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Prologue { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Epilogue { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Summary { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Publisher { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string PublishYear { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string PublishPlace { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Contributor> Creators { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Contributor> Contributors { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Topics { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> Tags { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<string> KeyWords { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<BookResult> Related { get; set; }


        public void Merge(Book from)
        {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
