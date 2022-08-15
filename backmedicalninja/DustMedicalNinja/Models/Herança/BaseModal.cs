using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.Heranca
{
    [DataContract]
    public class BaseModal
    {
        [DataMember]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [DataMember]
        public bool status { get; set; }

        [DataMember]
        public Log log { get; set; }
    }
}
