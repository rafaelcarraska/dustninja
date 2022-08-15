using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Acesso
    {
        [DataMember]
        public string empresaId { get; set; }

        [DataMember]
        public List<Tela> listaTela { get; set; }
    }
}