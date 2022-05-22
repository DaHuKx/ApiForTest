using ApiForTest.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApiForTest
{
    public class BaseTest : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public BaseTest(DbContextOptions<BaseTest> options) : base(options)
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
