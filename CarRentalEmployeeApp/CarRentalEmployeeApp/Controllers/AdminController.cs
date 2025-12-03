using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CarRentalEmployeeApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly CarRentalDbContext _context;
        public AdminController(CarRentalDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetCarAll()
        {

            List<Vehicle> vehicles = await _context.Vehicles.ToListAsync();

            return View(vehicles);
        }
        [HttpPost]
        public async Task<IActionResult> Getcar(int id)
        {
            var _id = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);

            if (_id == null)
            {
                return NotFound("ARAÇ BULUNAMADI");
            }

            return View(_id);
        }
        [HttpGet]
        public async Task<IActionResult> Createcar()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCar(CreateVehicleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                                .Where(x => x.Value.Errors.Count > 0)
                                .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) });

                foreach (var err in errors)
                {
                    Console.WriteLine($"{err.Field}: {string.Join(", ", err.Errors)}");
                }

                return View(model);
            }



            // ViewModel’den Entity oluşturdum
            var vehicle = new Vehicle
            {
                PlateNumber = model.PlateNumber,
                CarModel = model.CarModel,
                Year = model.Year,
                kilometer = model.kilometer,
                Status = model.Status
            };

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction("AdminDashboard","Admin");
        }


        [HttpGet]
        public async Task<IActionResult> Updatecar(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound("ARAÇ BULUNAMADI");
            }
            return View(vehicle);



        }
        [HttpPost]
        public async Task<IActionResult> Updatecar(Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return View(vehicle);
            }
            var existingVehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicle.Id);
            if (existingVehicle == null)
            {
                return NotFound("ARAÇ BULUNAMADI");
            }
            existingVehicle.PlateNumber = vehicle.PlateNumber;
            existingVehicle.CarModel = vehicle.CarModel;               // tek tek kontrol etmmemi sağlıyor 
            existingVehicle.Year = vehicle.Year;
            existingVehicle.kilometer = vehicle.kilometer;
            existingVehicle.Status = vehicle.Status;
            await _context.SaveChangesAsync();
            return RedirectToAction("GetCarAll");

        }
        [HttpPost]
        public async Task<IActionResult> Deletecar(int id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
            {
                return NotFound("ARAÇ BULUNAMADI");
            }
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetCarAll");
        }
        public async Task<IActionResult> AdminDashboard()
        {
            var totalVehicles = await _context.Vehicles.CountAsync();
            var rentedVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Rented);
            var availableVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Available);
            var maintenanceVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Maintenance);
            var totalEmployees = await _context.Employee.CountAsync();

            var employees = await _context.Employee.ToListAsync();
            var vehicles = await _context.Vehicles.ToListAsync();

            var dashboardData = new AdminDashboardViewModel
            {
                TotalVehicles = totalVehicles,
                RentedVehicles = rentedVehicles,
                AvailableVehicles = availableVehicles,
                MaintenanceVehicles = maintenanceVehicles,
                TotalEmployees = totalEmployees,
                Employees = employees,
                Vehicles = vehicles
            };

            return View(dashboardData);
        }

        
        public async Task<IActionResult> GetEmploye()

        {
            var employess = await _context.Employee.ToListAsync();

            return View(employess);


        }
        [HttpGet]
        public async Task<IActionResult> CreateEmploye()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmploye(Employee employe)
        {
            if (!ModelState.IsValid)
            {
                return View(employe);
            }
            _context.Employee.Add(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetEmploye");
                
        }
        [HttpGet]
        public async Task<IActionResult> UpdateEmploye(string id)
        {
            var updateemploye = await _context.Employee.FirstOrDefaultAsync(e => e.Id == id);
            if (updateemploye == null)
            {
                return NotFound("ÇALIŞAN BULUNAMADI");
            }
            return View(updateemploye);

        }

        public async Task<IActionResult> UpdateEmploye(Employee employe)
        {

            if (!ModelState.IsValid)
            {
                return View(employe);

            }
            _context.Employee.Update(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetEmploye");


        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmploye(string id)
        {
            var employe = await _context.Employee.FirstOrDefaultAsync(e => e.Id ==id);
            if (employe == null)
            {
                return NotFound("ÇALIŞAN BULUNAMADI");
            }
            _context.Employee.Remove(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetEmploye");
        }
    }
}


