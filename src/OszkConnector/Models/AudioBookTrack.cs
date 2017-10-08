using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    [DataContract]
    public class AudioBookTrack
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public Uri FileUrl { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Int64 Length { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Size { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string MetaData { get; set; }

        public override string ToString()
        {
            return $"{FileName} - {Title}";
        }
    }
}
