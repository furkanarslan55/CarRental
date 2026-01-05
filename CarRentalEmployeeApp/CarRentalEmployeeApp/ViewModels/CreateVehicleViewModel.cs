using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CreateVehicleViewModel
    {
        [Required]
        public string PlateNumber { get; set; }

        [Required]
        public string CarModel { get; set; }

        [Required(ErrorMessage = "Marka seçilmelidir")]
        public int BrandId { get; set; }
        [ValidateNever] // KULLANICIDAN MANUEL VERİ ALMADIĞIM İÇİN MODELSTATE TARAFINDA KONTOL EDİLMESİNİ ÖNLEMEK İÇİN KULLANILIR
        public List<SelectListItem> Brands { get; set; }

        [Required(ErrorMessage = "Araç türü zorunludur")]
        public string Type { get; set; }

        public GearType GearType { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Range(0, int.MaxValue)]
        public int Kilometer { get; set; }

        public VehicleStatus Status { get; set; }

        public bool HasDamage { get; set; }

        public List<CreateVehicleDamageViewModel> Damages { get; set; } = new();
    }

}
