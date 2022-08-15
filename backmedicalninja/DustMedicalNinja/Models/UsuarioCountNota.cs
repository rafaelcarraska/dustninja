using DustMedicalNinja.Extensions;
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
    public class UsuarioCountNota : BaseModal
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMId { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string usuarioId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ultimaLeitura { get; set; }

        [DataMember]
        public string ultimaLeitura_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", ultimaLeitura);
            }
        }
    }
}
