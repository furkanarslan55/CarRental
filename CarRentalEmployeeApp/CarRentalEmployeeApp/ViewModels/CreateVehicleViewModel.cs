using CarRentalEmployeeApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CreateVehicleViewModel
    {
        [Required(ErrorMessage ="Plaka numarası zorunludur")]
        [StringLength(10,ErrorMessage ="En fazla 10 karakter olmalıdır")]
        [Display(Name ="Plaka Numarası")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage ="Model alanı zorunludur")]
     
        public string CarModel { get; set; }

        [Required]
        [Range(1900, 2100, ErrorMessage = "Geçerli bir yıl girin.")]
        public int Year { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Kilometre sıfırdan küçük olamaz.")]
        public int kilometer { get; set; } 

        [Required]
        public VehicleStatus Status { get; set; } 
    }

}
