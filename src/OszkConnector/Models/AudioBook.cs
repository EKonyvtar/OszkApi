using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class AudioBook : Book
    {
        public string PlayListFile { get; set; }
        public List<AudioBookTrack> Tracks { get; set; }
    }
}
