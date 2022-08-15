using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class DadosBasicos
    {
        [DataMember]
        public string usuarioNome { get; set; }

        [DataMember]
        public string usuarioId { get; set; }

        [DataMember]
        public string empresaLogada { get; set; }        

        [DataMember]
        public List<string> roles { get; set; }

        [DataMember]
        public string picture { get; set; }

        [DataMember]
        public bool master { get; set; }

        [DataMember]
        public Configuracao configuracao { get; set; }
    }
}
