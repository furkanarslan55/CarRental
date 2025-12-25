using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CarRentalEmployeeApp.Controllers
{
    public class RentalController : Controller
    {

        private readonly CarRentalDbContext _context;
        private readonly UserManager<Employee> _userManager;//kullanıcı yönetimi için
        public RentalController(CarRentalDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> RentalCar()

        {
            var employee = await _userManager.GetUserAsync(User);

            if (employee == null) 
                {

                return RedirectToAction("Login", "Account");
                  }


            var assignedVehicles = await _context.Vehicles
                .Where(v => v.AssignedToId == employee.Id && v.Status ==VehicleStatus.flexible)
                .ToListAsync();




            return View(assignedVehicles);
        }


        [HttpGet]
        public async Task<IActionResult> Rent(int id)
        {
            var employee = await _userManager.GetUserAsync(User);
            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null || vehicle.AssignedToId != employee.Id || vehicle.Status != VehicleStatus.flexible)
            {
                return NotFound();
            }

            var model = new VehicleRentalCreateViewModel
            {
                VehicleId = vehicle.Id,
                PlateNumber = vehicle.PlateNumber,
                CarModel = vehicle.CarModel,
                Year = vehicle.Year,
                Kilometer = vehicle.kilometer



            };


            return View(model);
        }



        [HttpPost]

        public async Task<IActionResult> RentalCar(VehicleRentalCreateViewModel model)
        {
            var employee = await _userManager.GetUserAsync(User);


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null ||
               vehicle.AssignedToId != employee.Id ||
               vehicle.Status != VehicleStatus.flexible)
            {
                return View("Rent",model);
            }

            var rental = new Rental
            {
                VehicleId = model.VehicleId,
                CustomerId = model.CustomerId,
                StartRental = model.StartRental,
                EndRental = model.EndRental,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                EmployeeId = employee.Id
            };



            vehicle.Status = VehicleStatus.busy;

            _context.Rentals.Add(rental);
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("RentalCar");
        }





















    }
}
