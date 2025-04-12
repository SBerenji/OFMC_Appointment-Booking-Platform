using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Controllers
{
    public class AccountController : Controller
    {
        // SignInManager instance is injected for login.
        private readonly SignInManager<User> _signInManager;

        // UserManager instance is injected for registration.
        private readonly UserManager<User> _userManager;

        private readonly HealthcareDbContext _healthcareDbContext;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, HealthcareDbContext healthcareDbContext)
        {
            this._signInManager = signInManager;

            this._userManager = userManager;

            this._healthcareDbContext = healthcareDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }


        //[Authorize(Roles = "Patient")]
        [HttpGet("/Login/Patient/Form")]
        public IActionResult PatientLoginForm()
        {
            return View("PatientLogin");
        }


        [HttpPost("/Login/Patient")]
        public async Task<IActionResult> PatientLogin(LoginViewModel LoginInfo)
        {
            if (ModelState.IsValid) {
                var result = await this._signInManager.PasswordSignInAsync(LoginInfo.Email, LoginInfo.Password, LoginInfo.RememberMe, false);

                if (result.Succeeded) {
                    var user = await this._userManager.FindByEmailAsync(LoginInfo.Email);

                    if (!await this._userManager.IsInRoleAsync(user, "Patient"))
                    {
                        await this._userManager.AddToRoleAsync(user, "Patient");
                    };

                    // Needs to be Linked.
                    return RedirectToAction("GetAllAppointments", "HealthCare");
                }

                else
                {
                    ModelState.AddModelError("", "Failed to login");
                };
            };

            // going back to the login form.
            return View(LoginInfo);
        }



        [HttpPost("/Register/Patient")]
        public async Task<IActionResult> PatientRegistration(RegisterViewModel RegistrationInfo)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();

                newUser.UserName = RegistrationInfo.Email;
                newUser.FirstName = RegistrationInfo.FirstName;
                newUser.LastName = RegistrationInfo.LastName;
                newUser.DateOfBirth = RegistrationInfo.DateOfBirth;
                newUser.PhoneNumber = RegistrationInfo.PhoneNumber.Replace("-", "");
                newUser.Email = RegistrationInfo.Email;

                // Here we attempt to register the new account.
                var result = await this._userManager.CreateAsync(newUser, RegistrationInfo.Password);

                if (result.Succeeded)
                {
                    if (!await this._userManager.IsInRoleAsync(newUser, "Patient"))
                    {
                        await this._userManager.AddToRoleAsync(newUser, "Patient");
                    }

                    var newPatient = new Patient
                    {
                        UserId = newUser.Id,
                        PatientEmail = newUser.Email,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        DOB = newUser.DateOfBirth
                    };

                    _healthcareDbContext.Patient.Add(newPatient);
                    await _healthcareDbContext.SaveChangesAsync();



                    await this._signInManager.SignInAsync(newUser, true);

                    return RedirectToAction("GetAllAppointments", "Healthcare");
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }


            return View("Register", RegistrationInfo);
        }


        public IActionResult AdminLoginForm()
        {
            return View("AdminLogin");
        }


        [HttpPost("/Login/Admin")]
        public async Task<IActionResult> AdminLogin(LoginViewModel LoginInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await this._signInManager.PasswordSignInAsync(LoginInfo.Email, LoginInfo.Password, LoginInfo.RememberMe, false);

                if (result.Succeeded)
                {
                    var user = await this._userManager.FindByEmailAsync(LoginInfo.Email);

                    if (!await this._userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await this._userManager.AddToRoleAsync(user, "Admin");
                    };

                    Console.WriteLine("Redirecting Admin....");

                    return RedirectToAction("GetDoctorsList", "Admin");
                }

                else
                {
                    ModelState.AddModelError("", "Failed to login");
                };
            };

            // going back to the login form.
            return View(LoginInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
