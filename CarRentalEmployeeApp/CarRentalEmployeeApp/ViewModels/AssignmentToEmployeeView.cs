using CarRentalEmployeeApp.Models;

namespace CarRentalEmployeeApp.ViewModels
{
    public class AssignmentToEmployeeView
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; }
        public string CarModel { get; set; }
        public int Year { get; set; }
        public int Kilometers { get; set; }



        public string SelectedEmployeeId { get; set; }
        public List<Employee> EmployeName  { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;







    }
}
