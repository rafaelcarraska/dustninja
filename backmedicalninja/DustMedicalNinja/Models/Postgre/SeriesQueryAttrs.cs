using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("series_query_attrs")]
    public class SeriesQueryAttrs
    {
        [Key]
        public int pk { get; set; }
        public string retrieve_aets { get; set; }
        public int series_fk { get; set; }
    }
}
