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
    public class Log
    {
        [DataMember]
        public string insertUsuarioId { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime insertData { get; set; }

        [DataMember]
        public string updateUsuarioId { get; set; }

        [DataMember]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime updateData { get; set; }

        [DataMember]
        public string insertDataFormatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", insertData);
            }
        }

        [DataMember]
        public string updateDataEncerramento
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", updateData);
            }
        }
    }
}
