using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.Models
{
    public enum GearType
    {
        [Display(Name = "Manuel")]
        Manuel = 1,
        [Display(Name = "Otomatik")]
        Otomatik = 2,
        [Display(Name = "Yarı Otomatik")]
        YariOtomatik = 3


    }
    public enum VehicleStatus
    {
        [Display(Name = "Müsait")]
        Flexible = 0,

        [Display(Name = "Kirada")]
        Busy = 1,

        [Display(Name = "Bakımda")]
        Maintenance = 2
    }


    public enum AssignmentStatus
    {

        free,
        appointed

    }
    [Index(nameof(PlateNumber), IsUnique = true)]

    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(10, ErrorMessage = "Plaka en fazla 10 karakter olabilir.")]
        public string PlateNumber { get; set; }
        public int BrandId { get; set; }
        public Brand CarBrand { get; set; }

        [Required(ErrorMessage = "Araç modeli zorunludur.")]
        [StringLength(50, ErrorMessage = "Araç modeli en fazla 50 karakter olabilir.")]
        public string? CarModel { get; set; }

        [Range(1900, 2100, ErrorMessage = "Model yılı 1900 ile 2100 arasında olmalıdır.")]
        public int? Year { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Kilometre negatif olamaz.")]
        public int? Kilometer { get; set; }
        public string Type { get; set; }

        public GearType GearType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal DailyPrice { get; set; }
        public VehicleStatus Status { get; set; } = VehicleStatus.Flexible;

        public AssignmentStatus AssignmentStatus { get; set; } = AssignmentStatus.free;

        // Sorumlu çalışan
        public string? AssignedToId { get; set; }
        public ICollection<VehicleDamage> Damages { get; set; } = new List<VehicleDamage>();

        public virtual Employee? AssignedTo { get; set; }
    }

}