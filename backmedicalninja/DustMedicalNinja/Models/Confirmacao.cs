using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;
using DustMedicalNinja.Models.ViewModel;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class Confirmacao : BaseModal
    {
        [DataMember]
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMId { get; set; }

        [DataMember]
        [Required]
        public string prioridade { get; set; }

        [DataMember]
        public string statusExamesAnterior { get; set; }

        [DataMember]
        public string subStatusExamesAnterior { get; set; }

        [DataMember]
        [Required]
        public string statusExames { get; set; }

        [DataMember]
        public string subStatusExames { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string templateImpressaoid { get; set; }

        [DataMember]
        public string historiaClinica { get; set; }

        [DataMember]
        public List<string> listaTipoEstudo { get; set; }

        [DataMember]
        public Log segundaLeitura { get; set; }

        [DataMember]
        public DesmembrarViewModel desmembrar { get; set; }
    }
}
