using CarRentalEmployeeApp.Models;


namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleDashboardViewModel
    {
        public List<Vehicle> AvailableVehicles { get; set; }
        public List<Vehicle> RentedVehicles { get; set; }
        public List<Vehicle> MaintenanceVehicles { get; set; }

    }
}
