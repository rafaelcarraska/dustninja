using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DustMedicalNinja.Models.Heranca;

namespace DustMedicalNinja.Models
{
    public class SyncNinja : GrupoEmpresa
    {
        public int study_fk { get; set; }
        public string filedcm { get; set; }
    }
}
