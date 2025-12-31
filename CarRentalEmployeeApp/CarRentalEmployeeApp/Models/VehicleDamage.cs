namespace CarRentalEmployeeApp.Models
{



    public enum DamageType
    {
        changing,      // Çizik
        painted,         // Göçük
      
    }
    public class VehicleDamage
    {

        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public DamageType DamageType { get; set; }
        public string Description { get; set; }

        public DateTime ReportDate { get; set; }
      

        public string? ImagePath { get; set; }

    }
}
