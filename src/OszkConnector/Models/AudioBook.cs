using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class AudioBook : Book
    {
        public List<AudioBookTrack> Tracks { get; set; }

        public string ToM3UPlayList()
        {
            //TODO: implement
            return null;
        }
    }
}
