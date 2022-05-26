using ApiForTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiForTest
{
    public class DataBase : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DataBase(DbContextOptions<DataBase> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Property(pr => pr._skills).HasColumnName("Skills");
        }

        public bool IsEmpty()
        {
            return Persons.Count() == 0;
        }
    }
}
