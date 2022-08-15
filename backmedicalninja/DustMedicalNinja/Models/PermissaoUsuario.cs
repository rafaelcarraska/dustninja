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
    public class PermissaoUsuario
    {
        [DataMember]
        public string usuarioId { get; set; }

        [DataMember]
        public List<Permissao> listaPermissao { get; set; }
    }
}