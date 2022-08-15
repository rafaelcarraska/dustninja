using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Msg
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public List<string> erro { get; set; }

        [DataMember]
        public bool status { get; set; }
    }
}
