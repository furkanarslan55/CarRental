using CarRentalEmployeeApp.Data;
using CarRentalEmployeeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalEmployeeApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly CarRentalDbContext _context;
        private readonly UserManager<Employee> _userManager;

       public HomeController(CarRentalDbContext context, UserManager<Employee> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        public IActionResult Index()
        {


            return View();
        }
    }
}
