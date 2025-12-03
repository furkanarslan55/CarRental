using CarRentalEmployeeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();





builder.Services.AddIdentity<Employee, IdentityRole>(
    
    Options =>
    {
        Options.Password.RequireDigit = true;
        Options.Password.RequireLowercase = true;  // þifre kurallarý   
        Options.Password.RequireUppercase = true;
        Options.Password.RequireNonAlphanumeric = false;
        Options.Password.RequiredLength = 6;
        Options.User.RequireUniqueEmail = true;
    }
    )
    
    .AddEntityFrameworkStores<CarRentalEmployeeApp.Data.CarRentalDbContext>()  //ýdentity kýsmý 
    .AddDefaultTokenProviders();



builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";  // Login olmayaný buraya yönlendir
});



builder.Services.AddDbContext<CarRentalEmployeeApp.Data.CarRentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  //veritabaný baðlantýsý


var app = builder.Build();



// Seed admin user and role

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string adminRole = "Admin";
    string adminEmail = "admin@carrental.com";
    string adminPassword = "Admin123!";

    if (!await roleManager.RoleExistsAsync("Employee"))
        await roleManager.CreateAsync(new IdentityRole("Employee"));

    if (!await roleManager.RoleExistsAsync(adminRole))
        await roleManager.CreateAsync(new IdentityRole(adminRole));

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new Employee
        {
            UserName = adminEmail,
            Email = adminEmail,
            Name = "Admin",
            Surname = "User",
            Address = "Varsayýlan Yönetici Adresi",
            PhoneNumber = "0000000000",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            LockoutEnabled = false

        };
        await userManager.CreateAsync(admin, adminPassword);
        await userManager.AddToRoleAsync(admin, adminRole);
    }
}















// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
