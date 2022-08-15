using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.ViewModel
{
    [DataContract]
    public class ValidaTwoFactorViewModel
    {
        [DataMember]
        public string usuarioId { get; set; }

        [DataMember]
        public int pin { get; set; }

    }

}
