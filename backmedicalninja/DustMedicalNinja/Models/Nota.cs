using DustMedicalNinja.Models.Heranca;
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
    public class Nota : GrupoEmpresa
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMId { get; set; }

        [DataMember]
        public List<NotaHistorico> listaNota { get; set; }
    }
}
