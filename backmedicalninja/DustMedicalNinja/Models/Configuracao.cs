using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Configuracao : BaseModal
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string usuarioId { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo com quantidade de itens por página é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de itens por página deve ser maior que zero.")]
        public int pageSize { get; set; }

        [DataMember]
        public string filtroPadrao { get; set; }
    }
}
