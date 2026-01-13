using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarRentalEmployeeApp.Models

{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Rental> Rentals { get; set; }
        public string? EmployeeId { get; set; }
        
        public Employee Employee { get; set; }
        [BindNever]
        public DateTime CreatedAt { get; set; }
        [BindNever]
        public string CreatedBy { get; set; }



    }
}
