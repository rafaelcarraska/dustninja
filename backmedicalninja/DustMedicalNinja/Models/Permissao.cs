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
    public class Permissao
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DataMember]
        public string facilityId { get; set; }

        [DataMember]
        public string usuarioId { get; set; }

        [DataMember]
        public List<string> listaPermissao{ get; set; }
    }    
}