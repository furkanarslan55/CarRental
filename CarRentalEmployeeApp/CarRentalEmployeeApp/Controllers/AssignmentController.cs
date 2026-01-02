using Microsoft.AspNetCore.Mvc;
using CarRentalEmployeeApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;

namespace CarRentalEmployeeApp.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AssignmentController : Controller
    {

        private readonly CarRentalDbContext _context;
     
        public AssignmentController(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AssignmentToCar()
        {
            var car = await _context.Vehicles
                .Where(v => v.AssignmentStatus == AssignmentStatus.free)
                .ToListAsync();

            return View(car);

        }

        public async Task<IActionResult> AssignmentToEmployee(int id)
        {
            var car = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (car == null && car.AssignmentStatus != AssignmentStatus.free)
            {
                return NotFound("Araç Zimmetli veya Araç yok");
            }

            var model = new AssignmentToEmployeeView
            {
                VehicleId = car.Id,
                PlateNumber = car.PlateNumber,
                CarModel = car.CarModel,
                Year = car.Year!.Value,
                EmployeName = await _context.Employee.ToListAsync(),
                Created = DateTime.Now




            };


            return View(model);



        }


        public async Task<IActionResult> AssignCarToEmployee(AssignmentToEmployeeView model)
        {
            var car = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == model.VehicleId);
            if (car == null && car.AssignmentStatus != AssignmentStatus.free)
            {
                return NotFound("Araç Zimmetli veya Araç yok");
            }
            car.AssignedToId = model.SelectedEmployeeId;

            car.AssignmentStatus = AssignmentStatus.appointed;
            _context.Vehicles.Update(car);
            await _context.SaveChangesAsync();

            return RedirectToAction("AssignmentToCar");

        }

    }
    }   



