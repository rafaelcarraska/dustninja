using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Models.Postgre
{
    [Table("person_name")]
    public class PersonName
    {
        [Key]
        public int pk { get; set; }

        public string family_name { get; set; }

        public string given_name { get; set; }

        public string middle_name { get; set; }

        public string name_prefix { get; set; }
    }
}
