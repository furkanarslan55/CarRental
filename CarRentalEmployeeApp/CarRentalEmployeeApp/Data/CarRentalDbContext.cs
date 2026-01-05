using CarRentalEmployeeApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;


namespace CarRentalEmployeeApp.Data

{
    public class CarRentalDbContext:  IdentityDbContext<Employee>
    {
        public DbSet<Customer> Customers { get; set; }
  
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<Rental>()
     .HasOne(r => r.Customer)
     .WithMany(c => c.Rentals)
     .HasForeignKey(r => r.CustomerId)
     .OnDelete(DeleteBehavior.Restrict);




        }

    }
}
