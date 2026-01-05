using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
