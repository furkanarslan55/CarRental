using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CustomerUpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        [Required(ErrorMessage = "Lütfen sorumlu personel seçiniz")]
        public string? EmployeeId { get; set; }

        // Dropdown listesi
        public List<SelectListItem> Employees { get; set; }
    }
}
