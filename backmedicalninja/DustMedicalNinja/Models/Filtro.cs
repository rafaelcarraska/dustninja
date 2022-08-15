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
    public class Filtro : GrupoEmpresa
    {
        [DataMember]
        [Required(ErrorMessage = "Campo descrição é obrigatório")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo descricao deve conter entre 3 e 50 caracteres.")]
        public string descricao { get; set; }

        [DataMember]
        public bool particular { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string usuarioId { get; set; }

        [DataMember]
        public List<FiltroStatus> listaFiltroStatus { get; set; }

        [DataMember]
        public List<FiltroDatas> listaFiltroDatas { get; set; }

        [DataMember]
        public List<FiltroGerais> listaFiltroGerais { get; set; }

        [DataMember]
        public List<FiltroOrdem> listaFiltroOrdem { get; set; }

        [DataMember]
        public List<FiltroPerfil> listaPerfil { get; set; }
    }

    public class FiltroPerfil
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string descricao { get; set; }
    }

    public class FiltroStatus
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string descricao { get; set; }
    }

    public class FiltroDatas
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string descricao { get; set; }

        [DataMember]
        public int dias { get; set; }
    }

    public class FiltroGerais
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string descricao { get; set; }

        [DataMember]
        public string valor { get; set; }
    }

    public class FiltroOrdem
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string descricao { get; set; }

        [DataMember]
        public string ordem { get; set; }
    }
}