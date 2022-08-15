using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class NotaHistoricoViewModel
    {
        [DataMember]
        public string nota { get; set; }

        [DataMember]
        public string usuarioId { get; set; }

        [DataMember]
        public string perfil { get; set; }

        [DataMember]
        public string usuario { get; set; }

        [DataMember]
        public DateTime data { get; set; }

        [DataMember]
        public string dataFormatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", data);
            }
        }
    }
}
