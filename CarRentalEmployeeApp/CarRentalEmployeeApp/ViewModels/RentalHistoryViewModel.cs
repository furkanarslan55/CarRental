namespace CarRentalEmployeeApp.ViewModels
{
    public class RentalHistoryViewModel
    {
        // Rental
        public int RentalId { get; set; }
        public DateTime StartRental { get; set; }
        public DateTime EndRental { get; set; }
        public int TotalDays { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        // Vehicle
        public int VehicleId { get; set; }
        public string PlateNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string GearType { get; set; }

        // Customer
        public int CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Employee
        public string EmployeeFullName { get; set; }

        // Damage
        public bool HasDamage { get; set; }
    }

}
