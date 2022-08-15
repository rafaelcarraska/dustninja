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
    public class WebConfig : BaseModal
    {
        public bool sincronizando { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ultimaSincronizacao { get; set; }

        public int timeoutSincronizacao { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime dataLimiteSincronizacao
        {
            get
            {
                return ultimaSincronizacao.AddMinutes(timeoutSincronizacao);
            }
        }

        public int LimiteRefresh { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime dataLimiteRefresh
        {
            get
            {
                return ultimaSincronizacao.AddMinutes(LimiteRefresh);
            }
        }
    }
}
