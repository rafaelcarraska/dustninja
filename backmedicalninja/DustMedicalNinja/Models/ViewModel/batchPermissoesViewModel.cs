using DustMedicalNinja.Models.Heranca;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class BatchPermissoesViewModel
    {
        [DataMember]
        public string facilityId { get; set; }

        [DataMember]
        public string usuarioId { get; set; }

        [DataMember]
        public List<Combobox> listUsuarios { get; set; }

        [DataMember]
        public List<Combobox> listaPermissoes { get; set; }

    }
}
