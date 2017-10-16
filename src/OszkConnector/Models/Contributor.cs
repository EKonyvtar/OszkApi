using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class Contributor
    {
        public bool IsFamilyFirst { get; set; } = true;
        public string Role { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; } 

        public override string ToString()
        {
            return IsFamilyFirst?
                $"{FamilyName} {GivenName}":
                $"{GivenName} {FamilyName}";
        }
    }
}
