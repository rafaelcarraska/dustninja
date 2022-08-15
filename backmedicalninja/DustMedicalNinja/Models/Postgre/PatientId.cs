using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("patient_id")]
    public class PatientId
    {
        [Key]
        public int pk { get; set; }

        public string pat_id { get; set; }
    }
}
