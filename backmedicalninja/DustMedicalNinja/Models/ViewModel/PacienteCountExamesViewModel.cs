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
    public class PacienteCountExamesViewModel
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string pacienteId { get; set; }

        public string aeTitle { get; set; }        

        [DataMember]
        public int? countExames { get; set; }
    }
}
