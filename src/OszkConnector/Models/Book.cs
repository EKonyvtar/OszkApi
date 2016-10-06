using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class Book : BookResult
    {
        public string Contents { get; set; }
        public string Prologue { get; set; }
        public string Epilogue { get; set; }
        public string Summary { get; set; }
        public string Contributor { get; set; }
        public List<string> Tags { get; set; }

        public void Merge(Book from)
        {
            //TODO:
            throw new NotImplementedException();
        }
    }
}
