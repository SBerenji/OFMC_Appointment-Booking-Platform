using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//get the db connection string 
string? dbConn = builder.Configuration.GetConnectionString("OFMC_DB");

//add our db context class as a service to the DI container
builder.Services.AddDbContext<HealthcareDbContext>(options => options.UseSqlServer(dbConn));


builder.Services.AddDefaultIdentity<User>(
    options =>

    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 8;

        options.User.RequireUniqueEmail = true;
    }
)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<HealthcareDbContext>();


// registering the Admin Service.
builder.Services.AddScoped<IAdminService, AdminService>();

// registering the Patient Service.
builder.Services.AddScoped<IPatientService, PatientService>();

// registering the Account Service.
builder.Services.AddScoped<IAccountService, AccountService>();

// registering the email service
builder.Services.AddScoped<IEmailService, EmailService>();

// registering the SMS service
builder.Services.AddScoped<ISmsService, TwilioSmsService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapStaticAssets();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();



using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Patient" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }


    var config = builder.Configuration;
    
    var adminUsers = config.GetSection("AdminUsers").Get<List<AdminUserConfig>>();


    foreach (var admin in adminUsers)
    {
        if (string.IsNullOrWhiteSpace(admin.Email) || string.IsNullOrWhiteSpace(admin.Password))
            continue;

        var existingUser = await userManager.FindByEmailAsync(admin.Email);
        if (existingUser == null)
        {
            var user = new User
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                UserName = admin.Email,
                Email = admin.Email,
                EmailConfirmed = true,
                DateOfBirth = string.IsNullOrWhiteSpace(admin.DOB) ? DateTime.Today : DateTime.Parse(admin.DOB).Date
            };

            var result = await userManager.CreateAsync(user, admin.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");

                Console.WriteLine($"Seeded admin user: {admin.Email}");
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating user {admin.Email}: {error.Description}");
                }
            }
        }
    }
}





app.Run();