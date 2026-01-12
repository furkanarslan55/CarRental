using System.ComponentModel.DataAnnotations;
using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.ViewModels
{
    public class UpdateCarViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(10, ErrorMessage = "Plaka en fazla 10 karakter olabilir.")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Marka seçimi zorunludur.")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Araç modeli zorunludur.")]
        [StringLength(50)]
        public string? CarModel { get; set; }

        [Range(1900, 2100)]
        public int? Year { get; set; }

        [Range(0, int.MaxValue)]
        public int? Kilometer { get; set; }

        [Required(ErrorMessage = "Araç tipi zorunludur.")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Vites tipi seçiniz.")]
        public GearType GearType { get; set; }

        public VehicleStatus Status { get; set; }

        public AssignmentStatus AssignmentStatus { get; set; }

        // Aracı sorumlu çalışana atama
        public string? AssignedToId { get; set; }

        // Araç hasarları (opsiyonel: edit ekranda gösterim için)
        public List<VehicleDamageViewModel> Damages { get; set; } = new();
    }
}
