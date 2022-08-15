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
    public class Facility : GrupoEmpresa
    {
        [DataMember]
        [Required(ErrorMessage = "Campo descrição é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo descrição deve conter entre 3 e 50 caracteres.")]
        public string descricao { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo razão social é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo razão social deve conter entre 3 e 50 caracteres.")]
        public string razaoSocial { get; set; }

        [DataMember]
        public string aeTitle { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo tempo de retencao das imagens é obrigatório")]
        public int tempoRetencaoImagens { get; set; }

        [DataMember]
        public Endereco endereco { get; set; }

        [DataMember]
        public List<Contato> listaContato { get; set; }

        [DataMember]
        public Prioridade prioridade { get; set; }

        [DataMember]
        [StringLength(500, ErrorMessage = "O campo nota radiologista deve conter no máximo 500 caracteres.")]
        public string notaRadiologista { get; set; }

        [DataMember]
        public List<string> listaTemplateImpressao { get; set; }
    }
}
