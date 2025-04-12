using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

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


        // Defining a method that gets all the appointments associated with a doctor and returns to the viewmodel
        [HttpGet]
        public IActionResult GetDoctorAppointments(int doctorId)
        {
            Doctor doctor = _healthcareDbContext.Doctor  // finding the doctor based on the doctor id passed to the action
                .Include(d => d.Appointments)
                .FirstOrDefault(d => d.DoctorId == doctorId);

            if (doctor == null)
            {
                return NotFound();
            }

            DoctorAppointmentsViewModel viewModel = new DoctorAppointmentsViewModel  // initialized the view model with the required information
            {
                DoctorId = doctor.DoctorId,
                DoctorName = doctor.DoctorName,
                Appointments = doctor.Appointments
                    .Where(a => a.AppointmentDate >= DateTime.Now)  // only get the upcoming appointments
                    .OrderBy(a => a.AppointmentDate)
                    .Select(a => new AppointmentInfoViewModel
                    {
                        PatientName = a.PatientName,
                        AppointmentDate = a.AppointmentDate
                    })
                    .ToList()
            };

            return View("../Admin/DoctorAppointments", viewModel);
        }




        //[HttpGet("/doctorAppointmentDetails")]
        //public IActionResult GetDoctorAppointmentDetails()
        //{
        //    //to be implemented later
        //}



        //[HttpGet("/cancelAppointment")]
        //public IActionResult CancelPatientAppointment()
        //{
        //    //to be implemented later
        //}

    }
}
