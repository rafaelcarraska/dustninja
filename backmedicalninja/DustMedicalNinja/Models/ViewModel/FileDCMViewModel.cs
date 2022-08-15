using DustMedicalNinja.Models;
using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class FileDCMViewModel : StatusExameViewModel
    {
        [DataMember]
        public int pkPostgre { get; set; }

        [DataMember]
        public string fileName { get; set; }

        [DataMember]
        public string studyId { get; set; }

        [DataMember]
        public string studyDesc { get; set; }

        [DataMember]
        public string aeTitle { get; set; }

        [DataMember]
        public string body_part { get; set; }

        [DataMember]
        public string institution { get; set; }

        [DataMember]
        public string department { get; set; }

        [DataMember]
        public string modality { get; set; }

        [DataMember]
        public string notaRadiologista { get; set; }

        [DataMember]
        public string series_desc { get; set; }
                
        [DataMember]
        public bool pendente { get; set; }

        [DataMember]
        public bool favorito { get; set; }

        public DateTime date_study { get; set; }

        [DataMember]
        public string date_study_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", date_study);
            }
        }

        public DateTime dateConfirmacao { get; set; }

        [DataMember]
        public string dateConfirmacaoFormatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", dateConfirmacao);
            }
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string pacienteId { get; set; }

        [DataMember]
        public string pacienteNome { get; set; }

        [DataMember]
        public Paciente paciente { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string facilityId { get; set; }

        [DataMember]
        public string facilityDesc { get; set; }

        [DataMember]
        public bool permitirDownload { get; set; }

        [DataMember]
        public bool removerNotas { get; set; }

        [DataMember]
        public bool permitirLaudo { get; set; }
        
        [DataMember]
        public bool confirmarExame { get; set; }

        [DataMember]
        public string tempoParaLaudar { get; set; }

        [DataMember]
        public long countAnexo { get; set; }

        [DataMember]
        public long countNota { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string templateImpressaoUtilizado { get; set; }

        public List<Confirmacao> historicoExame { get; set; }

        [DataMember]
        public List<string> listaTipoEstudo { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMPai { get; set; }
    }
}
