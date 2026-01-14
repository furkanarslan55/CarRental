using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CarRentalEmployeeApp.ViewModels
{
    public class VehicleRentalPageViewModel
    {

        [ValidateNever]
        public string PlateNumber { get; set; }
        [ValidateNever]
        public string CarModel { get; set; }
        public int? Year { get; set; }
        public int? Kilometer { get; set; }


        public VehicleRentalCreateViewModel Rental { get; set; }
    }
}
