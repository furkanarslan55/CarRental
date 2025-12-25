using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.Models
{
    public enum VehicleStatus
    {
       
            flexible,
            busy,
            manintance,

    }
    public enum AssignmentStatus
    {
        
        free,
            appointed

    }

    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plaka zorunludur.")]
        [StringLength(10, ErrorMessage = "Plaka en fazla 10 karakter olabilir.")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Araç modeli zorunludur.")]
        [StringLength(50, ErrorMessage = "Araç modeli en fazla 50 karakter olabilir.")]
        public string CarModel { get; set; }

        [Range(1900, 2100, ErrorMessage = "Model yılı 1900 ile 2100 arasında olmalıdır.")]
        public int Year { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Kilometre negatif olamaz.")]
        public int kilometer { get; set; }

        [Required(ErrorMessage = "Durum seçilmelidir.")]
        public VehicleStatus Status { get; set; } = VehicleStatus.flexible;

        public AssignmentStatus AssignmentStatus { get; set; } = AssignmentStatus.free;

        // Sorumlu çalışan
        public string? AssignedToId { get; set; }

        public virtual Employee? AssignedTo { get; set; }
    }
}
