using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Combobox
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string descricao { get; set; }

        [DataMember]
        public string filtro1 { get; set; }

        [DataMember]
        public string filtroId1 { get; set; }

        [DataMember]
        public DateTime dataInclusao { get; set; }

        [DataMember]
        public string dataInclusaoFormatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataInclusao);
            }
        }
    }
}
