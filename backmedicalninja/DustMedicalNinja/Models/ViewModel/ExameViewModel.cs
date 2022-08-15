using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class ExameInfoViewModel
    {
        [DataMember]
        public string fileDCMId { get; set; }

        [DataMember]
        public string usuario { get; set; }

        [DataMember]
        public string usuarioId { get; set; }        

        [DataMember]
        public string login { get; set; }

        [DataMember]
        public string data { get; set; }
    }
}
