using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class TemplateImpressaoViewModel : GrupoEmpresa
    {
        [DataMember]
        public string descricao { get; set; }

        [DataMember]
        public string cabecalho { get; set; }

        [DataMember]
        public bool repetiCabecalho { get; set; }

        [DataMember]
        public string corpo { get; set; }

        [DataMember]
        public string rodape { get; set; }

        [DataMember]
        public bool repetiRodape { get; set; }

        [DataMember]
        public int headerHeight { get; set; }

        [DataMember]
        public int footerHeight { get; set; }

        [DataMember]
        public long countAnexo { get; set; }
    }
}