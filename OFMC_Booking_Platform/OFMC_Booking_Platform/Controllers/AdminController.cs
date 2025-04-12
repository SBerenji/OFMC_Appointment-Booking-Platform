using Microsoft.AspNetCore.Mvc;
using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Controllers
{
    public class AdminController : Controller
    {

        private HealthcareDbContext _healthcareDbContext; //private HealthcareDbContext variable


        // constructor - tells the DI container that this controller needs a DB context 
        public AdminController(HealthcareDbContext healthcareDbContext)
        {
            _healthcareDbContext = healthcareDbContext; //intitalizes the controller with a reference to the HealthcareDbContext
        }

        // GET handler for the list of all of the doctors on the admin side
        [HttpGet("/doctorsList")] //specifies the URL - GET handler for the list of all of the doctors
        public IActionResult GetDoctorsList()
        {
            //retrieves a list of doctors from the database
            List<Doctor> doctors = _healthcareDbContext.Doctor.ToList();

            return View("../Admin/DoctorList", doctors);  //returns the list of doctors to the view using the view name
        }



        [HttpGet("/getAppointments")]
        public IActionResult GetDoctorAppointments()
        {
            //will implement later

            return View("../Admin/");
        }




    }
}
