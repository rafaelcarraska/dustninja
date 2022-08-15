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
    public class Usuario : BaseModal
    {
        [DataMember]
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo nome deve conter entre 3 e 50 caracteres.")]
        public string nome { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo login é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo login deve conter entre 3 e 20 caracteres.")]
        public string login { get; set; }

        public string senha { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo e-mail é obrigatório")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "O campo e-mail deve conter entre 5 e 30 caracteres.")]
        [EmailAddress(ErrorMessage = "Esse e-mail não é valido.")]
        public string email { get; set; }

        [DataMember]
        public string perfilId { get; set; }        

        [DataMember]
        public Endereco endereco { get; set; }

        [DataMember]
        public bool master { get; set; }

        [DataMember]
        public List<Contato> listaContato { get; set; }        

        [DataMember]
        public Assinatura assinatura { get; set; }

        [DataMember]
        public List<string> listaEmpresa { get; set; }

        [DataMember]
        public bool twofactor { get; set; }

        public string key { get; set; }

    }
}
