using DustMedicalNinja.Models;
using DustMedicalNinja.Models.Postgre;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DustMedicalNinja.Context
{
    public class DCMContext : DbContext
    {
        public DbSet<Study> Study { get; set; }
        public DbSet<StudyQueryAttrs> StudyQueryAttrs { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PatientId> PatientId { get; set; }
        public DbSet<PersonName> PersonName { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<SeriesQueryAttrs> SeriesQueryAttrs { get; set; }
        public DCMContext(DbContextOptions<DCMContext> options) :
            base(options)
        {
        }
    }
}
