using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("series")]
    public class Series
    {
        [Key]
        public int pk { get; set; }
        public string body_part { get; set; }
        public string institution { get; set; }
        public string department { get; set; }
        public string modality { get; set; }
        public string series_desc { get; set; }
        public string src_aet { get; set; }
        public string station_name { get; set; }

        [ForeignKey("study_fk")]
        public int study_fk { get; set; }
    }
}
