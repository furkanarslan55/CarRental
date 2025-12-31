using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    [Index(nameof(PhoneNumber), IsUnique = true)]
    public class UpdateEmployeViewModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        [MaxLength(11, ErrorMessage ="Lütfen 11 haneli telefon numarası giriniz")]
        [MinLength(11, ErrorMessage = "Lütfen 11 haneli telefon numarası giriniz")]
        
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
