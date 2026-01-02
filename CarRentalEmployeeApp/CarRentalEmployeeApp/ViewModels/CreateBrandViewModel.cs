using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CreateBrandViewModel
    {
        [Required(ErrorMessage = "Marka adı zorunludur")]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
