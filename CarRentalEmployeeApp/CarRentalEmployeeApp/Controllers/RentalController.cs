using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.EntityFrameworkCore;

public class RentalController : Controller
{
    private readonly CarRentalDbContext _context;
    private readonly UserManager<Employee> _userManager;

    public RentalController(CarRentalDbContext context, UserManager<Employee> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 1️⃣ Kiralanabilir araçlar listesi
    [HttpGet]
    public async Task<IActionResult> RentalCar()
    {
        var employee = await _userManager.GetUserAsync(User);
        if (employee == null)
            return RedirectToAction("Login", "Account");

        var vehicles = await _context.Vehicles
            .Where(v => v.AssignedToId == employee.Id &&
                        v.Status == VehicleStatus.Flexible)
            .Select(v => new VehicleRentalListVM
            {
                VehicleId = v.Id,
                PlateNumber = v.PlateNumber,
                CarModel = v.CarModel,
                Year = v.Year!.Value,
                Kilometer = v.Kilometer!.Value,
                DailyPrice = v.DailyPrice

            })
            .ToListAsync();

        return View(vehicles);
    }
    [HttpGet]
    public async Task<IActionResult> Rent(int vehicleId)
    {
        var employee = await _userManager.GetUserAsync(User);
        if (employee == null)
            return RedirectToAction("Login", "Account");

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == vehicleId);

        if (vehicle == null)
            return NotFound();

        // Admin her aracı görebilir
        if (!User.IsInRole("Admin") &&
            vehicle.AssignedToId != employee.Id)
            return Forbid();

        var model = new VehicleRentalPageViewModel
        {
            PlateNumber = vehicle.PlateNumber,
            CarModel = vehicle.CarModel,
            Year = vehicle.Year,
            Kilometer = vehicle.Kilometer,
            Rental = new VehicleRentalCreateViewModel
            {
                VehicleId = vehicle.Id,
                DailyPrice = vehicle.DailyPrice,
                Customers = await GetCustomersSelectList()
            }
        };

        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Rent(VehicleRentalPageViewModel pageModel)
    {
        var employee = await _userManager.GetUserAsync(User);
        if (employee == null)
            return RedirectToAction("Login", "Account");

        // 🔑 Formdan gelen asıl model
        var model = pageModel.Rental;

        var vehicle = await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == model.VehicleId);

        if (vehicle == null)
            return NotFound();

        // ❌ Validation
        if (!ModelState.IsValid || model.StartRental >= model.EndRental)
        {
            if (model.StartRental >= model.EndRental)
                ModelState.AddModelError("", "Bitiş tarihi başlangıçtan sonra olmalıdır.");

            // View tekrar render edileceği için eksik alanları doldur
            pageModel.PlateNumber = vehicle.PlateNumber;
            pageModel.CarModel = vehicle.CarModel;
            pageModel.Year = vehicle.Year;
            pageModel.Kilometer = vehicle.Kilometer;
            pageModel.Rental.Customers = await GetCustomersSelectList();

            return View(pageModel);
        }

        // ✅ Yetki kontrolü
        if (!User.IsInRole("Admin") &&
            vehicle.AssignedToId != employee.Id)
            return Forbid();

        if (vehicle.Status != VehicleStatus.Flexible)
            return BadRequest();

        // ✅ Kayıt
        var rental = new Rental
        {
            VehicleId = model.VehicleId,
            CustomerId = model.CustomerId,
            StartRental = model.StartRental,
            EndRental = model.EndRental,
            DailyPrice = model.DailyPrice,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            EmployeeId = employee.Id
        };

        vehicle.Status = VehicleStatus.Busy;

        _context.Rentals.Add(rental);
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();

        return RedirectToAction("RentalCar");
    }



    private async Task<List<SelectListItem>> GetCustomersSelectList()
    {
        return await _context.Customers
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
    }

    [HttpGet]
    public async Task<IActionResult> ActiveRent()
    {

        var employee = await _userManager.GetUserAsync(User);
        if (employee == null)
            return RedirectToAction("Login", "Account");

        var roles = await _userManager.GetRolesAsync(employee);


        if (roles.Contains("Admin"))
        {

            var model = _context.Rentals
                .Where(r => r.IsActive)
                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .Select(r => new ActiveRentalListVM
                {
                    RentalId = r.Id,
                    PlateNumber = r.Vehicle!.PlateNumber,
                    CarModel = r.Vehicle!.CarModel,
                    CustomerName = r.Customer!.Name,
                    StartRental = r.StartRental,
                    EndRental = r.EndRental,
                    DailyPrice = r.DailyPrice
                });

            var activeRentals = await model.ToListAsync();
            return View(activeRentals);



        }

        else if (roles.Contains("Employee"))

        {
            var model = _context.Rentals
                .Where(r => r.IsActive && r.Vehicle!.AssignedToId == employee.Id)
                .Include(r => r.Vehicle)
                .Include(r => r.Customer)
                .Select(r => new ActiveRentalListVM
                {
                    RentalId = r.Id,
                    PlateNumber = r.Vehicle!.PlateNumber,
                    CarModel = r.Vehicle!.CarModel,
                    CustomerName = r.Customer!.Name,
                    StartRental = r.StartRental,
                    EndRental = r.EndRental,
                    DailyPrice = r.DailyPrice
                });
            var activeRentals = await model.ToListAsync();



            return View(activeRentals);

        }



        return Forbid();




    }


}
