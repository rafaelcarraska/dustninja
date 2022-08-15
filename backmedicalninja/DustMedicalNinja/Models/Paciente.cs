using DustMedicalNinja.Extensions;
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
    public class Paciente : GrupoEmpresa
    {
        [DataMember]
        public int pkPostgre { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime dataNascimento { get; set; }

        [DataMember]
        public string nome { get; set; }

        [DataMember]
        public string namePrefix { get; set; }

        [DataMember]
        public string middleName { get; set; }

        [DataMember]
        public string giveName { get; set; }

        [DataMember]
        public string pacienteIdDCM { get; set; }

        [DataMember]
        public string sexo { get; set; }

        [DataMember]
        public string nomeCompleto {
            get
            {
               return new string($"{namePrefix} {nome} {middleName} {giveName}").Trim();
            }
        }

        [DataMember]        
        public string dataNascimento_formatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", dataNascimento);
            }
        }

        [DataMember]
        public string idade
        {
            get
            {
                return dataNascimento.Idade().ToString();
            }
        }
    }
}
