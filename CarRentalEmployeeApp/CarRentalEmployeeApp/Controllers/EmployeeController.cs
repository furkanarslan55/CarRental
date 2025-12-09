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
    [Authorize(Roles ="Employee, Admin")]
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





        // Dashboard: kendi müşteri listesini görür
        [HttpGet]
        public async Task<IActionResult> DashboardCustomer()
        {
            var employee = await _userManager.GetUserAsync(User); //employe içindeki ıd yi alıyoruz


            var customers = _context.customers
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
                    .Where(v => v.AssignedToId == employee.Id && v.Status == VehicleStatus.Rented)
                    .ToListAsync(),
                MaintenanceVehicles = await _context.Vehicles
                    .Where(v => v.AssignedToId == employee.Id && v.Status == VehicleStatus.Maintenance)
                    .ToListAsync()


            };     
            return View(model);
        }


        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerCreateViewModel model)
        {
            var employee = await _userManager.GetUserAsync(User);
            if (!ModelState.IsValid)
                return View(model);
          
            if (employee == null)
                return Unauthorized();

           var createcustomer = new Customers
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmployeeId = employee.Id
            };

            _context.customers.Add(createcustomer);

            await _context.SaveChangesAsync();

            return RedirectToAction("DashboardCustomer");
        }

    



        [HttpGet]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var employee = await _userManager.GetUserAsync(User);
            if (employee == null)
                return Unauthorized();

            var customer = await _context.customers
                .FirstOrDefaultAsync(c => c.EmployeeId == employee.Id && c.Id == id);

            if (customer == null)
                return NotFound();

            return View(customer);
        }




        [HttpPost]
        public async Task<IActionResult> GetCustomerUpdate(Customers customers)
        {
            if (!ModelState.IsValid)
                return View(customers);

            var employee = await _userManager.GetUserAsync(User);

            var existingCustomer = await _context.customers
                .FirstOrDefaultAsync(x => x.Id == customers.Id && x.EmployeeId == employee.Id);

            if (existingCustomer == null)
                return NotFound();

            existingCustomer.Name = customers.Name;
            existingCustomer.Email = customers.Email;
            existingCustomer.PhoneNumber = customers.PhoneNumber;
            existingCustomer.Address = customers.Address;

            await _context.SaveChangesAsync();

            return RedirectToAction("DashboardCustomer");
        }

        














    }
}
