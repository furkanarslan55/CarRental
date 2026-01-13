using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarRentalEmployeeApp.Controllers
{
    [Authorize(Roles ="Employee ")]
    public class EmployeeController : Controller
    {

        private readonly CarRentalDbContext _context;
        private readonly UserManager<Employee> _userManager;//kullanıcı yönetimi için
        public EmployeeController(CarRentalDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public  async Task<IActionResult> EmployeeDashboard()
        {

            return View();
        }





       [HttpGet]
        public async Task<IActionResult> DashboardCustomer()
        {
            var employee = await _userManager.GetUserAsync(User); //employe içindeki ıd yi alıyoruz


            var customers = _context.Customers
                .Where(c => c.EmployeeId == employee.Id)
                .ToList();

            return View(customers);
        }






        [HttpGet]
        public async Task<IActionResult> DashboardVehicle()
        {
            var employee = await _userManager.GetUserAsync(User);

            if (employee == null)
            {
                return Unauthorized();

            }

            var model = new VehicleDashboardViewModel  // Bir view model kullanarak koleksiyon olarak verileri geçiyorum.
            {
                AvailableVehicles = await _context.Vehicles
                    .Where(v => v.AssignedToId == employee.Id)
                    .ToListAsync(),
                RentedVehicles = await _context.Vehicles
                    .Where(v => v.AssignedToId == employee.Id && v.Status == VehicleStatus.Busy)
                    .ToListAsync(),
                MaintenanceVehicles = await _context.Vehicles
                    .Where(v => v.AssignedToId == employee.Id && v.Status == VehicleStatus.Maintenance)
                    .ToListAsync()


            };     
            return View(model);
        }


        
    



        














    }
}
