namespace CarRentalEmployeeApp.ViewModels
{
    public class ExtendRentalVM
    {
        public int RentalId { get; set; }

        public string PlateNumber { get; set; }
        public string CustomerName { get; set; }

        public DateTime StartRental { get; set; }
        public DateTime CurrentEndDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public decimal DailyPrice { get; set; }

        // Hesaplanan alanlar
        public int ExtraDays =>
            Math.Max((NewEndDate.Date - CurrentEndDate.Date).Days, 0);

        public decimal ExtraPrice => ExtraDays * DailyPrice;
    }
}
