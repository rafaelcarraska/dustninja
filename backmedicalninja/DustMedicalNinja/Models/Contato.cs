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
    public class Contato
    {
        [DataMember]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "O Campo Nome deve conter entre 3 e 30 caracteres")]
        public string contato { get; set; }

        [DataMember]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O Campo Nome deve conter entre 3 e 30 caracteres")]
        public String empresa { get; set; }

        [DataMember]
        [StringLength(20, ErrorMessage = "O Campo telefone deve conter no max. 20 caracteres")]
        //[Phone(ErrorMessage = "Digite um telefone valido")]
        public string telefone { get; set; }

        [DataMember]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "O Campo E-mail deve conter entre 3 e 80 caracteres")]
        [EmailAddress(ErrorMessage = "O campo E-mail é obrigatorio.")]
        public String email { get; set; }
    }
}
