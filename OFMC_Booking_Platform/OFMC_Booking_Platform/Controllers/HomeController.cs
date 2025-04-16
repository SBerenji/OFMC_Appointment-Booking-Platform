using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;

            this._userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if ((User.Identity != null) && (User.Identity.IsAuthenticated))
            {
                var user = await this._userManager.GetUserAsync(User);

                if (user != null)
                {
                    if (await this._userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("GetDoctorsList", "Admin");
                    }

                    else if (await this._userManager.IsInRoleAsync(user, "Patient"))
                    {
                        return RedirectToAction("GetAllAppointments", "Healthcare");
                    };
                };
            };

            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
