using Microsoft.AspNetCore.Mvc;
using OFMC_Booking_Platform.Entities;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Models;
using System.IO;

namespace OFMC_Booking_Platform.Controllers
{
    public class HealthcareController : Controller //controller for the healthcare information 
    {
        // constructor - tells the DI container that this controller needs a DB context 
        public HealthcareController(HealthcareDbContext healthcareDbContext)
        {
            _healthcareDbContext = healthcareDbContext; //intitalizes the controller with a reference to the HealthcareDbContext
        }

        // GET handler for the list of all of the doctors
        [HttpGet("/doctors")] //specifies the URL - GET handler for the list of all of the doctors
        public IActionResult GetAllDoctors()
        {
            //retrieves a list of doctors from the database
            List<Doctor> doctors = _healthcareDbContext.Doctor.ToList();

            return View("../Healthcare/Manage", doctors);  //returns the list of doctors to the view using the view name
        }

        // GET handler for the list of all of the appointments
        [HttpGet("/appointments")] //specifies the URL - GET handler for the list of all of the appointments
        public IActionResult GetAllAppointments()
        {
            //retrieves a list of appointments from the database
            List<Appointment> appointments = _healthcareDbContext.Appointment
                .Include(m => m.Doctor) // Include the related Doctor data
                .ToList();

            return View("../Healthcare/Appointments", appointments);  //returns the list of appointments to the view using the view name
        }



        [HttpGet("/doctor/book-appointment-form")] //specifies the URL - GET handler for the blank add form
        public IActionResult GetAppointmentForm(int id)
        {
            // tries to find doctor with the id from the database
            Doctor? doctor = _healthcareDbContext.Doctor.Find(id);



            // populates the appointmentViewModel with the existing doctor info 
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                ActiveDoctor = doctor,
                ActiveAppointment = new Appointment()
            };

            // return that appointmentViewModel to the view
            return View("../Healthcare/BookAppointment", appointmentViewModel);
        }


        [HttpGet("/doctor/reschedule-appointment-form")] //specifies the URL - GET handler for the blank reschedule form
        public IActionResult GetRescheduleAppointmentForm(int id)
        {
            // tries to find appointment and doctor with the id from the database
            Appointment? appointment = _healthcareDbContext.Appointment.Find(id);
            Doctor? doctor = _healthcareDbContext.Doctor.Find(appointment.DoctorId);

            // populates the appointmentViewModel with the existing appointment info and doctor
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                ActiveDoctor = doctor,
                ActiveAppointment = appointment,
            };

            // return that appointmentViewModel to the view
            return View("../Healthcare/RescheduleAppointment", appointmentViewModel);
        }

        //POST handler that enables adding a appointment 
        [HttpPost("/doctor/book-appointment")]
        public IActionResult BookAppointment(AppointmentViewModel appointmentViewModel, int appointmentSlot)
        {
            appointmentViewModel.ActiveAppointment.DoctorId = appointmentViewModel.ActiveDoctor.DoctorId;
            appointmentViewModel.ActiveAppointment.PatientId = 1;

            if (ModelState.IsValid) //enter this if model state is valid 
            {
                _healthcareDbContext.Appointment.Add(appointmentViewModel.ActiveAppointment);   // add the appointment information in the database 

                _healthcareDbContext.SaveChanges(); // saves the changes to the database



                // redirect to the GetAllDoctors
                return RedirectToAction("GetAllDoctors", "Healthcare");
            }
            else
            {
                return View("../Healthcare/BookAppointment", appointmentViewModel);  // model is invalid so show errors and load the Add view
            }
        }

        [HttpPost("/doctor/reschedule-appointment")]
        public IActionResult RescheduleAppointment(AppointmentViewModel appointmentViewModel)
        {
            appointmentViewModel.ActiveAppointment.DoctorId = appointmentViewModel.ActiveDoctor.DoctorId;
            appointmentViewModel.ActiveAppointment.PatientId = 1;


            if (ModelState.IsValid) //enter this if model state is valid 
            {
                _healthcareDbContext.Appointment.Update(appointmentViewModel.ActiveAppointment);   // update the appointment information in the database 

                _healthcareDbContext.SaveChanges(); // saves the changes to the database


                // redirect to the GetAllDoctors 
                return RedirectToAction("GetAllDoctors", "Healthcare");
            }
            else
            {
                return View("../Healthcare/RescheduleAppointment", appointmentViewModel);  // model is invalid so show errors and load the Edit view
            }
        }


        private HealthcareDbContext _healthcareDbContext; //private HealthcareDbContext variable
    }
}