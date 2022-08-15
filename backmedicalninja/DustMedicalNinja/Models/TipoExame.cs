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
    public class TipoExame : GrupoEmpresa
    {
        [DataMember]
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome deve conter entre 3 e 100 caracteres.")]
        public string nome { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo modalidade é obrigatório")]
        [StringLength(5, ErrorMessage = "O campo modalidade deve conter no max. 5 caracteres.")]
        public string modalidade { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo código cobrança é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo código cobrança deve conter no max. 100 caracteres.")]
        public string codCobranca { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo palavra chave é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo palavra chave deve conter no max. 100 caracteres.")]
        public string palavraChave { get; set; }

        [DataMember]
        public string mascaraLaudoId { get; set; }
    }
}