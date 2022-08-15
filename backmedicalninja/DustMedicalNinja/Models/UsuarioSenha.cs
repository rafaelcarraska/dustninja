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
    public class UsuarioSenha
    {
        [DataMember]
        public Usuario usuario { get; set; }

        [DataMember]
        public string senha { get; set; }
    }
}
