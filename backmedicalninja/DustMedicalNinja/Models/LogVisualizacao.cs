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
    public class LogVisualizacao
    {
        [DataMember]
        public string tipo { get; set; }

        [DataMember]
        public string fileDCMId { get; set; }
        
    }
}
