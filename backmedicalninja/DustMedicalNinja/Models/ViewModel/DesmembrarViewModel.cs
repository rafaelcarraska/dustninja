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
    public class DesmembrarViewModel
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMId { get; set; }

        [DataMember]
        public string novaDescricao { get; set; }

        [DataMember]
        public List<string> novosExames { get; set; }

        [DataMember]
        public bool confirmacao { get; set; }
    }
}