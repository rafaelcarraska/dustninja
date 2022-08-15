using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Autenticacao
    {
        [DataMember]
        public string login { get; set; }

        [DataMember]
        public string senha { get; set; }

        [DataMember]
        public int pin { get; set; }

        [DataMember]
        public string empresaId { get; set; }
    }
}
