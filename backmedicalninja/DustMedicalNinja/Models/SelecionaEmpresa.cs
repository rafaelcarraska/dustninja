using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class SelecionaEmpresa
    {
        [DataMember]
        public IEnumerable<Combobox> comboBox { get; set; }

        [DataMember]
        public bool twofactor { get; set; }

        [DataMember]
        public string erro { get; set; }
    }
}
