using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class Book : BookResult
    {
        public string Urn { get; set; }
        public string MekId { get; set; }
        public Uri Source { get; set; }
        public List<string> Topics { get; set; }
        public string Language { get; set; }
        public string Period { get; set; }
        public string Contents { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string Summary { get; set; }
        public Publisher Publisher { get; set; }
        public object Creators { get; set; }
        public object Contributors { get; set; }
        public List<string> Tags { get; set; }
        public List<string> KeyWords { get; set; }

        public List<BookResult> Related { get; set; }
        public void Merge(Book from)
        {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
