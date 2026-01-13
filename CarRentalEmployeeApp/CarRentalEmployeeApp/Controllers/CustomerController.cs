using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRentalEmployeeApp.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class CustomerController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly CarRentalDbContext _context;
        public CustomerController(UserManager<Employee> userManager, CarRentalDbContext context)   // dependency injection
        {
            _userManager = userManager;
            _context = context;
        }





        [HttpGet]
        public async Task<IActionResult> CreateCustomer()
        {
            var employees = await _context.Users
                .Select(e => new
                {
                    e.Id,
                    FullName = string.IsNullOrEmpty(e.Name) && string.IsNullOrEmpty(e.Surname)
                        ? e.UserName
                        : e.Name + " " + e.Surname
                })
                .ToListAsync();

            ViewBag.Employees = new SelectList(employees, "Id", "FullName");

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = new SelectList(
                    await _context.Users.Select(e => new
                    {
                        e.Id,
                        FullName = string.IsNullOrEmpty(e.Name) && string.IsNullOrEmpty(e.Surname)
                            ? e.UserName
                            : e.Name + " " + e.Surname
                    }).ToListAsync(),
                    "Id",
                    "FullName"
                );

                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var customer = new Customer
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmployeeId = model.EmployeeId, // 🔥 SORUMLU PERSONEL
                CreatedAt = DateTime.UtcNow,
                CreatedBy = $"{currentUser.Name} {currentUser.Surname}"
            };
            var roles = await _userManager.GetRolesAsync(currentUser);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            if (roles.Contains("Admin"))
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else if (roles.Contains("Employee"))
            {
                return RedirectToAction("EmployeeDashboard", "Employee");
            }

            return RedirectToAction("GetCustomerAll");
        }

        public async Task<IActionResult> GetCustomerAll()
        {
            var model = await _context.Customers
                .Include(c => c.Employee)
                .Select(x => new CustomerViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Surname = x.Surname,
                    Email = x.Email,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,

                    EmployeeName = x.Employee == null
                        ? "-"
                        : string.IsNullOrEmpty(x.Employee.Name)
                            ? x.Employee.UserName
                            : x.Employee.Name + " " + x.Employee.Surname
                })
                .ToListAsync();

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(int Id)
        {
            var exictingcustomer = await _context.Customers.FindAsync(Id);
            if (exictingcustomer == null) {


                return View(exictingcustomer);
            
            
            }
        _context.Customers.Remove(exictingcustomer);
            _context.SaveChanges();
            return RedirectToAction("GetCustomerAll");
       



        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int Id)
        {
            var customer =await _context.Customers.FindAsync(Id); // find methodu primary key ile arama yapar.FirstOrDefault a göre daha hızlıdır.

            if (customer == null)
            {


                return NotFound("Müşteri bulunamadı");


            }
            var model = new CustomerUpdateViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Surname = customer.Surname,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                EmployeeId = customer.EmployeeId,

                Employees = await _context.Employee
              .Select(e => new SelectListItem
              {
                  Value = e.Id.ToString(),
                  Text = e.Name + " " + e.Surname
              })
              .ToListAsync()
            };

            return View(model);






        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(CustomerUpdateViewModel model)


        {
            if (!ModelState.IsValid)
            {
                model.Employees = await _context.Employee
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = e.Name + " " + e.Surname
                    })
                    .ToListAsync();

                return View(model);
            }
            var user = await _context.Customers.FirstOrDefaultAsync(u => u.Id == model.Id);
            if (user == null) {


                return NotFound("Kullanıcı Bulunamadı");



            }

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
        user.EmployeeId = model.EmployeeId;



            await _context.SaveChangesAsync();
            return RedirectToAction("GetCustomerAll");



                }




        public async Task<IActionResult> DetailsCustomer(int id)
        {


            var exictingcustomer = _context.Customers.Include(c=>c.Employee).
                FirstOrDefault(x => x.Id == id);
            if (exictingcustomer == null)
            {


                return View(exictingcustomer);


            }

            var model = new CustomerViewModel
            {
                Id = exictingcustomer.Id,
                Name = exictingcustomer.Name,
                Surname = exictingcustomer.Surname,
                Email = exictingcustomer.Email,
                Address = exictingcustomer.Address,
                PhoneNumber = exictingcustomer.PhoneNumber,
                CreatedAt = exictingcustomer.CreatedAt,
                CreatedBy = exictingcustomer.CreatedBy,
                EmployeeName = exictingcustomer.EmployeeId





            };

            return View(model);








        }



    }
}
