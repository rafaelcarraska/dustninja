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
    public class Anexo : GrupoEmpresa
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string fileDCMId { get; set; }

        [DataMember]
        public string arquivo { get; set; }

        [DataMember]
        public string tamanho
        {
            get
            {
                return size.longTobytes();
            }
        }

        [DataMember]
        public long size { get; set; }

        [DataMember]
        public string descricao { get; set; }

        [DataMember]
        public string extensao { get; set; }
    }
}
