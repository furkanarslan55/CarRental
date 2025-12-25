using Microsoft.AspNetCore.Identity;

namespace CarRentalEmployeeApp.Models
{
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
