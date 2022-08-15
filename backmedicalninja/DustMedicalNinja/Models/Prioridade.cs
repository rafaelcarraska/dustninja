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
    public class Prioridade
    {
        [DataMember]
        public int rotina { get; set; }

        [DataMember]
        public bool permitirUrgencia { get; set; }

        [DataMember]
        public int urgencia { get; set; }

        [DataMember]
        public bool permitirCritico { get; set; }

        [DataMember]
        public int critico { get; set; }
    }
}