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
    public class Assinatura
    {
        [DataMember]
        public string arquivo { get; set; }

        [DataMember]
        [StringLength(50, ErrorMessage = "O campo linha 1 deve conter no max. 50 caracteres.")]
        public string linha1 { get; set; }

        [DataMember]
        [StringLength(50, ErrorMessage = "O campo linha 2 deve conter no max. 50 caracteres.")]
        public string linha2 { get; set; }

        [DataMember]
        [StringLength(50, ErrorMessage = "O campo linha 3 deve conter no max. 50 caracteres.")]
        public string linha3 { get; set; }

    }
}