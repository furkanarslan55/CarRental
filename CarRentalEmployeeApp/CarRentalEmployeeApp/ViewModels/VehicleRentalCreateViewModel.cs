using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleRentalCreateViewModel
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DailyPrice { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartRental { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndRental { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();
    }


}