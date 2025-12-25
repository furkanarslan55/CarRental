using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleRentalCreateViewModel
    {


        public int VehicleId { get; set; }
        public string PlateNumber { get; set; }
        public string CarModel { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }




        // Müşteri
        [Required]
        public int CustomerId { get; set; }
        public List<SelectListItem> Customers { get; set; }

        // Tarihler
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartRental { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndRental { get; set; }


    }
}
