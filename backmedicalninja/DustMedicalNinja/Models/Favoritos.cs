using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DustMedicalNinja.Models
{
    public class Favoritos : GrupoEmpresa
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string usuarioId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string filedcmId { get; set; }
    }
}
