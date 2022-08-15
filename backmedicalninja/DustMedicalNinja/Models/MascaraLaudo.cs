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
    public class MascaraLaudo : GrupoEmpresa
    {
        [DataMember]
        [Required(ErrorMessage = "Campo descrição é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo descricao deve conter entre 3 e 50 caracteres.")]
        public string descricao { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo laudo é obrigatório")]
        public string laudo { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo modalidade é obrigatório")]
        [StringLength(5, ErrorMessage = "O campo modalidade deve conter no max. 5 caracteres.")]
        public string modalidade { get; set; }
    }
}