using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("patient")]
    public class Patient
    {
        [Key]
        public int pk { get; set; }

        public DateTime updated_time { get; set; }

        public int pat_name_fk { get; set; }

        public int patient_id_fk { get; set; }

        public string pat_birthdate { get; set; }

        public string pat_sex { get; set; }

        [ForeignKey("patient_id_fk")]
        public PatientId patientId { get; set; }

        [ForeignKey("pat_name_fk")]
        public PersonName personName { get; set; }
    }
}
