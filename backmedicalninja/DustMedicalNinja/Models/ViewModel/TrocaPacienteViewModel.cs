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
    public class TrocaPacienteViewModel
    {
        [DataMember]
        public string fileDCMId { get; set; }

        [DataMember]
        public string pacienteId { get; set; }
    }
}
