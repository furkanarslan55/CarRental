using CarRentalEmployeeApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CreateVehicleViewModel
    {
        [Required(ErrorMessage = "Plaka numarası zorunludur")]
        [StringLength(10, ErrorMessage = "En fazla 10 karakter olmalıdır")]
        [Display(Name = "Plaka Numarası")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Model alanı zorunludur")]
        [Display(Name = "Araç Modeli")]
        public string CarModel { get; set; }
        public string CarBrand { get; set; }
        public string Type { get; set; }

        [Range(1900, 2100, ErrorMessage = "Geçerli bir yıl girin.")]
        public int Year { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Kilometre sıfırdan küçük olamaz.")]
        public int Kilometer { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Araç durumu seçilmelidir")]
        public VehicleStatus Status { get; set; }

        // HASAR VAR MI?
        public bool HasDamage { get; set; }

        // HASAR LİSTESİ
        public List<CreateVehicleDamageViewModel> Damages { get; set; }
            = new List<CreateVehicleDamageViewModel>();
    }
}
