using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRentalEmployeeApp.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class Employee : IdentityUser
    {
        // IdentityUser zaten Id, UserName, Email, PasswordHash içerir

  
        public string Name { get; set; }
        public string Surname { get; set; } 
        public string Address { get; set; }

       
        public virtual ICollection<Vehicle> AssignedVehicles { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }

}
