using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("study")]
    public class Study
    {
        [Key]
        public int pk { get; set; }
        public DateTime created_time { get; set; }
        public string study_desc { get; set; }
        public string study_iuid { get; set; }

        public string study_date { get; set; }

        public string study_time { get; set; }

        [ForeignKey("study_fk")]
        public List<Series> series { get; set; }

        [ForeignKey("study_fk")]
        public List<StudyQueryAttrs> studyqueryattrs { get; set; }

        [ForeignKey("patient_fk")]
        public Patient patient{ get; set; }
    }
}
