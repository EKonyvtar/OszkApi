using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class Contributor
    {
        public string Role { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; } 

        public override string ToString()
        {
            return $"{FamilyName} {GivenName}";
        }
    }
}
