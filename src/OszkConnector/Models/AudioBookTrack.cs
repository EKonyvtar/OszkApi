using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class AudioBookTrack
    {
        public int Track { get; set; }
        public string FileName { get; set; }

        public Uri FileUrl { get; set; }
        public string Title { get; set; }
        public string Length { get; set; }

        public string Size { get; set; }
    }
}
