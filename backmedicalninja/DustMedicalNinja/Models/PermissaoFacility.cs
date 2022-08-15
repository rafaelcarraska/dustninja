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
    public class PermissaoFacility
    {
        [DataMember]
        public string facilityId { get; set; }

        [DataMember]
        public List<Permissao> listaPermissao { get; set; }
    }
}