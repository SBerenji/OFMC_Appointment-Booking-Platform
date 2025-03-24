using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//get the db connection string 
string? dbConn = builder.Configuration.GetConnectionString("OFMC_DB");

//add our db context class as a service to the DI container
builder.Services.AddDbContext<HealthcareDbContext>(options => options.UseSqlServer(dbConn));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
