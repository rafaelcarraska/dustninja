using DustMedicalNinja.Models.Heranca;
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
    public class Perfil : BaseModal
    {
        [DataMember]
        [Required(ErrorMessage = "Campo descrição é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo descricao deve conter entre 3 e 100 caracteres.")]
        public string descricao { get; set; }

        [DataMember]
        public Acesso acesso { get; set; }
    }
}