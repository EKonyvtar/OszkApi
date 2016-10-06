using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class Book : BookResult
    {
        public string Contributor { get; set; }
        public List<string> Tags { get; set; }
    }
}
