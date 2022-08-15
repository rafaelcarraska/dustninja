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
    public class Empresa : BaseModal
    {
        [DataMember]
        [Required(ErrorMessage = "Campo razão social é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo razão social deve conter entre 3 e 50 caracteres.")]
        public string razaoSocial { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo nome fantasia é obrigatório")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "O campo nome fantasia deve conter entre 3 e 10 caracteres.")]
        public string nomeFantasia { get; set; }

        [DataMember]
        [StringLength(19, ErrorMessage = "O campo cnpj deve conter no max. 19 caracteres.")]
        public string cnpj { get; set; }

        [DataMember]
        public Endereco endereco { get; set; }

        [DataMember]
        public string responsavel { get; set; }

        [DataMember]
        public List<Contato> listaContato { get; set; }
        
        [DataMember]
        public List<Tela> listaTela { get; set; }
    }
}
