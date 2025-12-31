using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class PersonelCreateViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        [Required]

        public string Email { get; set; }
        [MaxLength(11)]
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
