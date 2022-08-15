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
    public class EventoExameViewModel
    {
        [DataMember]
        public string usuario { get; set; }

        [DataMember]
        public string perfil { get; set; }

        public DateTime data { get; set; }

        [DataMember]
        public string data_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", data);
            }
        }

        [DataMember]
        public string acao { get; set; }    

        [DataMember]
        public string Obs { get; set; }
    }
}
