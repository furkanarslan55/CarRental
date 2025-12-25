namespace CarRentalEmployeeApp.Models
{
    public class Rental
    {



        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public DateTime StartRental { get; set; }
        public DateTime EndRental { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
