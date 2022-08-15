using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("study_query_attrs")]
    public class StudyQueryAttrs
    {
        [Key]
        public int pk { get; set; }

        public string retrieve_aets { get; set; }

        public string mods_in_study { get; set; }

        [ForeignKey("study_fk")]
        public int study_fk { get; set; }
    }
}
