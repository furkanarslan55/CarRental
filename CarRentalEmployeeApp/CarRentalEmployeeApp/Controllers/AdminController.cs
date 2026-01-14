using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CarRentalEmployeeApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly CarRentalDbContext _context;
        private readonly SignInManager<Employee> _signInManager;
        public AdminController(CarRentalDbContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetCarAll()
        {

            List<Vehicle> vehicles = await _context.Vehicles.
                Include(v => v.AssignedTo)
                .Include(v => v.CarBrand).
                ToListAsync();

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
        public async Task<IActionResult> CreateCar()
        {
            LoadGearTypes();
            var model = new CreateVehicleViewModel
            {
                Brands = LoadBrands()
            };
            return View(model);
        }

        // HELPER METOT
        private void LoadGearTypes()
        {
            ViewBag.GearTypes = Enum.GetValues(typeof(GearType))
                .Cast<GearType>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = ((int)x).ToString()
                })
                .ToList();
        }
        private List<SelectListItem> LoadBrands()
        {
            var brands = _context.Brands
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();

       
            Console.WriteLine($"📋 {brands.Count} adet marka yüklendi");
            foreach (var b in brands)
            {
                Console.WriteLine($"  - {b.Text} (ID: {b.Value})");
            }

            return brands;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCar(CreateVehicleViewModel model)

        {
            Console.WriteLine("🔥 CREATECAR POST ÇALIŞTI");
            Console.WriteLine($"📌 BrandId değeri: {model.BrandId}"); 

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"{entry.Key} -> {error.ErrorMessage}");
                    }
                }

                LoadGearTypes();
                model.Brands = LoadBrands();



                return View(model);
            }

            bool plateExists = await _context.Vehicles
                .AnyAsync(v => v.PlateNumber == model.PlateNumber);

            if (plateExists)
            {
                ModelState.AddModelError("PlateNumber", "Bu plakaya sahip bir araç zaten kayıtlı.");

                LoadGearTypes();
                model.Brands = LoadBrands();
                return View(model);
            }

            var vehicle = new Vehicle
            {
                PlateNumber = model.PlateNumber,
                BrandId = model.BrandId,
                CarModel = model.CarModel,
                Type = model.Type,
                Year = model.Year,
                Kilometer = model.Kilometer,
                GearType = model.GearType,
                Status = model.Status,
                DailyPrice = model.DailyPrice,

            };

            if (model.HasDamage && model.Damages != null && model.Damages.Any())
            {
                foreach (var damage in model.Damages)
                {
                    vehicle.Damages.Add(new VehicleDamage
                    {
                        DamageType = damage.DamageType,
                        Description = damage.Description,
                        ReportDate = DateTime.Now
                    });
                }
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetCarAll");
        }

        [HttpGet]
        public async Task<IActionResult> Updatecar(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.Damages)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
                return NotFound("ARAÇ BULUNAMADI");

            var model = new UpdateCarViewModel
            {
                Id = vehicle.Id,
                PlateNumber = vehicle.PlateNumber,
                BrandId = vehicle.BrandId,
                CarModel = vehicle.CarModel,
                Year = vehicle.Year,
                Kilometer = vehicle.Kilometer,
                Type = vehicle.Type,
                GearType = vehicle.GearType,
                Status = vehicle.Status,
                AssignmentStatus = vehicle.AssignmentStatus,
                AssignedToId = vehicle.AssignedToId,
                Damages = vehicle.Damages.Select(d => new VehicleDamageViewModel
                {
                    Id = d.Id,
                    DamageType = d.DamageType,
                    Description = d.Description,
                    ReportDate = d.ReportDate
                }).ToList()
            };

            ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
            ViewBag.Employees = new SelectList(_context.Employee, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Updatecar(UpdateCarViewModel vehicle)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
                ViewBag.Employees = new SelectList(_context.Employee, "Id", "FullName");
                return View(vehicle);
            }

            var existingVehicle = await _context.Vehicles
                .FirstOrDefaultAsync(a => a.Id == vehicle.Id);

            if (existingVehicle == null)
                return NotFound("ARAÇ BULUNAMADI");

            existingVehicle.PlateNumber = vehicle.PlateNumber;
            existingVehicle.BrandId = vehicle.BrandId;
            existingVehicle.CarModel = vehicle.CarModel;
            existingVehicle.Year = vehicle.Year;
            existingVehicle.Kilometer = vehicle.Kilometer;
            existingVehicle.Type = vehicle.Type;
            existingVehicle.GearType = vehicle.GearType;
            existingVehicle.Status = vehicle.Status;
            existingVehicle.AssignmentStatus = vehicle.AssignmentStatus;
            existingVehicle.AssignedToId = vehicle.AssignedToId;

            await _context.SaveChangesAsync();
            return RedirectToAction("GetCarAll");
        }


        
        [HttpPost]
        public async Task<IActionResult> Deletecar(int Id)
        {
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(a => a.Id == Id);
            if (vehicle == null)
            {
                return NotFound("ARAÇ BULUNAMADI");
            }
            if (vehicle.AssignmentStatus == AssignmentStatus.appointed || vehicle.Status == VehicleStatus.Busy)
            {

                TempData["Error"] = "Araç silinemez";
                return RedirectToAction("GetCarAll");
            }


            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetCarAll");
        }

        [HttpGet]
        public async Task<IActionResult> CreateBrand()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(CreateBrandViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("Name", "Marka adı boş olamaz.");
                return View(model);
            }

            var normalizedName = model.Name.Trim();

            bool brandExists = await _context.Brands
                .AnyAsync(x => x.Name == normalizedName);

            if (brandExists)
            {
                ModelState.AddModelError("Name", "Bu marka zaten mevcut.");
                return View(model);
            }

            var brand = new Brand
            {
                Name = normalizedName
            };

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("BrandList");
        }

        public async Task<IActionResult> BrandList()  //tüm araçları include etmektense n+1 problemi yaşamamak için sadece count bilgisini alıyorum
        {
            var model = _context.Brands
        .Select(b => new BrandListVM
        {
            Id = b.Id,
            Name = b.Name,
            CreatedDate = b.CreatedDate,
            VehicleCount = b.Vehicles.Count()
        })
        .ToList();

            return View(model);
        }


        public async Task<IActionResult> AdminDashboard()
        {
            var totalVehicles = await _context.Vehicles.CountAsync();
            var rentedVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Busy);
            var availableVehicles = await _context.Vehicles.CountAsync(v => v.Status == VehicleStatus.Flexible);
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
            var model = employess.Select(e => new EmployeeListViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Surname = e.Surname,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber

            }).ToList();

            return View(model);


        }
        [HttpGet]
        public async Task<IActionResult> CreateEmploye()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmploye(PersonelCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu email zaten kayıtlı.");
                return View(model);
            }

            var user = new Employee
            {
                Name = model.Name,
                Surname = model.Surname,
                Address = model.Address,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email


            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Employee rolünü atıyoruz
                await _userManager.AddToRoleAsync(user, "Employee");
               
                TempData["SuccessMessage"] = "Çalışan başarıyla eklendi";
                return RedirectToAction("AdminDashboard", "Admin"); // Admin paneline geri dön
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }




        
        [HttpGet]
        public async Task<IActionResult> UpdateEmploye(string Id)
        {
            var updateemploye = await _context.Employee.FirstOrDefaultAsync(e => e.Id == Id);
            if (updateemploye == null)
            {
                return NotFound("ÇALIŞAN BULUNAMADI");
            }
            var model = new UpdateEmployeViewModel
            {

                Name = updateemploye.Name,
                Surname = updateemploye.Surname,
                Email = updateemploye.Email,
                PhoneNumber = updateemploye.PhoneNumber,
                Address = updateemploye.Address
            };
            return View(model);

        }

        public async Task<IActionResult> UpdateEmploye(UpdateEmployeViewModel model)
        {
            var employe = await _context.Employee.FirstOrDefaultAsync(e => e.Id == model.Id);

            if (employe == null)
            {
                return NotFound("ÇALIŞAN BULUNAMADI");
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Veri uyuşmazlığı";
                return View(model);

            }



            employe.Name = model.Name;
            employe.Surname = model.Surname;
            employe.Email = model.Email;
            employe.PhoneNumber = model.PhoneNumber;
            employe.Address = model.Address;



            await _context.SaveChangesAsync();
            return RedirectToAction("GetEmploye");


        }

        public async Task<IActionResult> DetailsEmploye(string Id)
        {
            var employee = await _context.Employee
        .Include(e => e.AssignedVehicles)
        .FirstOrDefaultAsync(e => e.Id == Id);
            if (employee == null)
            {
                return NotFound("ÇALIŞAN BULUNAMADI");
            }
            var model = new EmployeeDetailViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Vehicles = employee.AssignedVehicles.Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Plate = v.PlateNumber,
                    Brand = v.CarModel
                }).ToList()
            };

            return View(model);






        }











        [HttpGet]
        public async Task<IActionResult> DeleteEmploye(string Id)
        {
            var employe = await _context.Employee
                .Include(e => e.AssignedVehicles)
                .FirstOrDefaultAsync(e => e.Id == Id);

            if (employe == null)
            {
                return NotFound("ÇALIŞAN BULUNAMADI");
            }

            if (employe.AssignedVehicles.Any())
            {
                TempData["ErrorMessage"] = "Bu çalışana zimmetli araç var, silinemez.";
                return RedirectToAction("GetEmploye");
            }

            _context.Employee.Remove(employe);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Çalışan başarıyla silindi.";
            return RedirectToAction("GetEmploye");
        }


    }
}


