namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleRentalListVM
    {
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; }
        public string CarModel { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public decimal DailyPrice { get; set; }
    }
}
