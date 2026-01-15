using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleReturnViewModel
    {
        // --- Readonly ---
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }

        // --- Return ---
        public DateTime? ReturnDate { get; set; }
        public bool HasDamage { get; set; }

        // --- Damage ---
        public DamageType? DamageType { get; set; }
        public string? DamageDescription { get; set; }
    }

}
