using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess_Layer.Models;

namespace DataAccess_Layer.Data
{
    public class DataContext:DbContext
    {

        /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       {
           optionsBuilder.UseSqlServer("ConectionString");
       }*/


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Department>Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().Property(e => e.Salary).HasColumnType("decimal(18,5)");
        }
    }
}
