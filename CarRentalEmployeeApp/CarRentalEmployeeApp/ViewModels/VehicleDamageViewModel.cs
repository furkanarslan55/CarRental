using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleDamageViewModel
    {
        public int Id { get; set; }

        public DamageType DamageType { get; set; }

        public string Description { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
