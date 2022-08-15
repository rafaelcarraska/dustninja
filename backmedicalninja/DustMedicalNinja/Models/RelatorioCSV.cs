using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models
{
    [DataContract]
    public class RelatorioCSV
    {
        [DataMember]
        public DateTime de { get; set; }

        [DataMember]
        public DateTime ate { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Campo separador é obrigatório")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "O campo login deve conter entre 1 e 3 caracteres.")]
        public string separador { get; set; }
    }
}
