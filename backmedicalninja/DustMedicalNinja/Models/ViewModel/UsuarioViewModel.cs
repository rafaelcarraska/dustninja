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
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string nome { get; set; }

        public string perfil { get; set; }

        public string perfilId { get; set; }

        public DateTime dataInclusao { get; set; }

        public List<string> listaEmpresa { get; set; }

        public string dataInclusaoFormatada
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", dataInclusao);
            }
        }

    }
}
