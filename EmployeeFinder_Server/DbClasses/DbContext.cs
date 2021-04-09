using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFinder_Server.DbClasses
{
    class DataBaseContext : DbContext
    {
        public DataBaseContext() : base("DbConnection")
        {
            Database.SetInitializer<DataBaseContext>(new ContextInializer());
        }

        public DbSet<Cities> Cities { get; set; }
        public DbSet<Specialisations> Specialisations { get; set; }
        public DbSet<Candidates> Candidates { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<CompaniesWishLists> CompaniesWishLists { get; set; }
        public DbSet<Messages> Messages { get; set; }
    }

    class ContextInializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext db)
        {
            Companies company = new Companies()
            {
                Name = "IT STEP Academy",
                Login = "STEP",
                Password = "step"
            };
            db.Companies.Add(company);
            db.SaveChanges();
        }
    }
}
