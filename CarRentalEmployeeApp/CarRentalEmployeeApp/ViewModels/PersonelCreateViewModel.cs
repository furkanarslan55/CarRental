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
     
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage= "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
