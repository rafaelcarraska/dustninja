using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class FileDCM : GrupoEmpresa
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
        public string series_desc { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime date_study { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime dateConfirmacao { get; set; }

        [DataMember]        
        public string date_study_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", date_study);
            }
        }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime data_envio { get; set; }

        [DataMember]
        public string data_envio_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", data_envio);
            }
        }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string pacienteId { get; set; }

        [DataMember]
        public string statusExames { get; set; }

        [DataMember]
        public string subStatusExames { get; set; }

        [DataMember]
        public string prioridade { get; set; }

        [DataMember]
        public List<Confirmacao> historicoExame { get; set; }

        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMPai { get; set; }
    }

    public enum StatusExames
    {
        transmissao,
        confirmar,
        laudar,
        laudando,
        laudado,
        desconsiderado,
        comparacao
    }

    public enum SubStatusExames
    {        
        novo,
        laudar_comPreLaudo,
        laudar_segundaLeitura,
        laudar_reiterpretacao,
        laudar_pendencia,
    }

    public enum TipoPrioridade
    {
        normal,
        urgente,
        critico
    }
}
