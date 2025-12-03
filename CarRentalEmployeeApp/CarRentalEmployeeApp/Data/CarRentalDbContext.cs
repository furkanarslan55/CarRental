using System;
using CarRentalEmployeeApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace CarRentalEmployeeApp.Data

{
    public class CarRentalDbContext:  IdentityDbContext<Employee>
    {
        public DbSet<Customers> customers { get; set; }
  
        public DbSet<Vehicle> Vehicles { get; set; }
     public DbSet<Employee> Employee { get; set; }

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           

          

                }

    }
}
