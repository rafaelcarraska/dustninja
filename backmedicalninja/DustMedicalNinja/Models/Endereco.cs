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
    public class Endereco
    {
        [DataMember]
        [StringLength(50, ErrorMessage = "O campo endereço deve conter no máximo 50 caracteres.")]
        public string endereco { get; set; }

        [DataMember]
        [StringLength(10, ErrorMessage = "O campo Número deve conter no máximo 10 caracteres.")]
        public string numero { get; set; }

        [DataMember]
        [StringLength(100, ErrorMessage = "O campo Bairro deve conter no máximo 100 caracteres.")]
        public string bairro { get; set; }

        [DataMember]
        [StringLength(100, ErrorMessage = "O campo Complemento deve conter no máximo 100 caracteres.")]
        public string complemento { get; set; }

        [DataMember]
        [StringLength(8, ErrorMessage = "O campo CEP deve conter no máximo 8 números.")]
        public string cep { get; set; }//Salvar o CEP com o '-'

        [DataMember]
        [StringLength(50, ErrorMessage = "O campo cidade deve conter no máximo 50 caracteres.")]
        public string cidade { get; set; }

        [DataMember]
        [StringLength(2, ErrorMessage = "O campo UF deve conter no máximo 2 caracteres.")]
        public string uf { get; set; }

        [DataMember]
        [StringLength(500, ErrorMessage = "O campo Obs. deve conter no máximo 500 caracteres.")]
        public string obs { get; set; }
    }
}
