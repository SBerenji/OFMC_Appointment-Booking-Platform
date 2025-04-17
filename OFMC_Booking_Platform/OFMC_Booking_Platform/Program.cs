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



builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/Home/Index";
    options.AccessDeniedPath = "/Home/Index";

    // Here we control how long the cookie lives if RememberMe is true.
    options.ExpireTimeSpan = TimeSpan.FromDays(14);

    options.SlidingExpiration = true;
});



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





    var patientUsers = config.GetSection("PatientUsers").Get<List<PatientUserConfig>>();

    foreach (var patient in patientUsers)
    {
        if (string.IsNullOrWhiteSpace(patient.PatientEmail))
            continue;

        var existingUser = await userManager.FindByEmailAsync(patient.PatientEmail);
        if (existingUser == null)
        {
            var user = new User
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                UserName = patient.PatientEmail,
                Email = patient.PatientEmail,
                EmailConfirmed = true,
                DateOfBirth = string.IsNullOrWhiteSpace(patient.DOB) ? DateTime.Today : DateTime.Parse(patient.DOB).Date,
                PhoneNumber = patient.PatientPhone
            };

            // Default password for seeded patients.
            var password = "Patient@123";

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Patient");

                var dbContext = scope.ServiceProvider.GetRequiredService<HealthcareDbContext>();

                dbContext.Patient.Add(new Patient
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DOB = user.DateOfBirth,
                    PatientEmail = user.Email,
                    PatientPhone = user.PhoneNumber
                });

                await dbContext.SaveChangesAsync();

                Console.WriteLine($"Seeded patient user: {patient.PatientEmail}");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating patient user {patient.PatientEmail}: {error.Description}");
                }
            }
        }
    }

}





app.Run();