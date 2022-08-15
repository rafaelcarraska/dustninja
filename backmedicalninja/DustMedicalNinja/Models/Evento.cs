using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Evento
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DataMember]
        public string empresaId { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string usuarioId { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime data { get; set; }

        [DataMember]
        public string tela { get; set; }

        [DataMember]
        public string acao { get; set; }        

        [DataMember]
        public Object valorAnterio { get; set; }

        [DataMember]
        public Object valorNovo { get; set; }

        [DataMember]
        public bool erro { get; set; }

        [DataMember]
        public string Obs { get; set; }

        [DataMember]
        public string exception { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string itemId { get; set; }

        public bool logUsuario { get; set; }
    }
}
