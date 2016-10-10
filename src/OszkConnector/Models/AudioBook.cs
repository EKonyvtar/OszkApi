using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    [DataContract]
    public class AudioBook : Book
    {
        [DataMember()]
        public List<AudioBookTrack> Tracks { get; set; }

        public string ToM3UPlayList()
        {
            //TODO: implement
            return null;
        }
    }
}
