namespace CarRentalEmployeeApp.ViewModels
{
    public class ActiveRentalListVM
    {
        public int RentalId { get; set; }
        public string PlateNumber { get; set; }
        public string CarModel { get; set; }

        public string CustomerName { get; set; }
        public DateTime StartRental { get; set; }
        public DateTime EndRental { get; set; }
        public decimal DailyPrice { get; set; }

        public int TotalDays => (EndRental - StartRental).Days;
        public decimal TotalPrice => TotalDays * DailyPrice;

    }
}
