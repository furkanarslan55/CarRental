using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.Models
{
    public class Brand
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

     
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
