using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int RentedVehicles { get; set; }
        public int MaintenanceVehicles { get; set; }
        public int TotalEmployees { get; set; }

        public List<Employee> Employees { get; set; } = new();
        public List<Vehicle> Vehicles { get; set; } = new();
    }

}
