using CarRentalEmployeeApp.Models;
using CarRentalEmployeeApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalEmployeeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Employee> _signInManager;  // kullanıcı girişi için
        private readonly UserManager<Employee> _userManager;
        public AccountController(SignInManager<Employee> signInManager, UserManager<Employee> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);

                TempData["LoginSuccess"] = "Giriş başarılı! Yönlendiriliyorsunuz...";

                if (roles.Contains("Admin"))
                {
                    TempData["RedirectUrl"] = Url.Action("AdminDashboard", "Admin");
                }
                else if (roles.Contains("Employee"))
                {
                    TempData["RedirectUrl"] = Url.Action("EmployeeDashboard", "Employee");
                }
                else
                {
                    TempData["RedirectUrl"] = Url.Action("Index", "Home");
                }

                return View(model); // aynı view'a dönüp mesajı göstereceğiz
            }

            ViewBag.LoginFailed = "E-posta veya şifre hatalı";
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register()
        {
            return View();
        }




        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var employee = new Employee
            {

                Name = model.Name,
                Surname = model.Surname,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                Address = model.Address

            };

            var result = await _userManager.CreateAsync(employee, model.Password);

            if (result.Succeeded)
            {
                // Employee rolünü atıyoruz
                await _userManager.AddToRoleAsync(employee, "Employee");
                TempData["SuccessMessage"] = "Çalışan başarıyla eklendi";
                return RedirectToAction("AdminDashboard","Admin"); // Admin paneline geri dön
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

    
        }
    }

