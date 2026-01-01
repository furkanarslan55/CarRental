using CarRentalEmployeeApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRentalEmployeeApp.ViewModels
{
    public class CreateVehicleDamageViewModel
    {

        [Required]
        public DamageType DamageType { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }
    }
}
