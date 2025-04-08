using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;

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

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}")
//    .WithStaticAssets();





app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Register}/{id?}")
    .WithStaticAssets();




// Load environment variables from .env
DotNetEnv.Env.Load();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Ensure Admin role exists
    var adminRole = "Admin";
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    // Seed Admin users from .env
    var adminUsers = new List<(string? firstName, string? lastName, string? email, string? password, string? DOB)>
    {
        (Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME_1"), Environment.GetEnvironmentVariable("ADMIN_LASTNAME_1"), Environment.GetEnvironmentVariable("ADMIN_EMAIL_1"), Environment.GetEnvironmentVariable("ADMIN_PASSWORD_1"), Environment.GetEnvironmentVariable("ADMIN_DOB_1")),
        (Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME_2"), Environment.GetEnvironmentVariable("ADMIN_LASTNAME_2"), Environment.GetEnvironmentVariable("ADMIN_EMAIL_2"), Environment.GetEnvironmentVariable("ADMIN_PASSWORD_2"), Environment.GetEnvironmentVariable("ADMIN_DOB_2")),
        (Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME_3"), Environment.GetEnvironmentVariable("ADMIN_LASTNAME_3"), Environment.GetEnvironmentVariable("ADMIN_EMAIL_3"), Environment.GetEnvironmentVariable("ADMIN_PASSWORD_3"), Environment.GetEnvironmentVariable("ADMIN_DOB_3")),
        (Environment.GetEnvironmentVariable("ADMIN_FIRSTNAME_4"), Environment.GetEnvironmentVariable("ADMIN_LASTNAME_4"), Environment.GetEnvironmentVariable("ADMIN_EMAIL_4"), Environment.GetEnvironmentVariable("ADMIN_PASSWORD_4"), Environment.GetEnvironmentVariable("ADMIN_DOB_4")),
    };

    foreach (var (firstName, lastName, email, password, DOB) in adminUsers)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            continue;

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser == null)
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                DateOfBirth = string.IsNullOrWhiteSpace(DOB) ? DateTime.Today : DateTime.Parse(DOB).Date
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, adminRole);

                Console.WriteLine($"Seeded admin user: {email}");
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating user {email}: {error.Description}");
                }
            }
        }
    }
}





app.Run();
