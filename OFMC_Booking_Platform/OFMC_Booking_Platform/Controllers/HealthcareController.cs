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
            Doctor? doctor = _healthcareDbContext.Doctor.Where(p => p.DoctorId == id).FirstOrDefault();

            //makes a list of available slots for that doctor
            List<Availability>? availableSlots = _healthcareDbContext.Availability.Where(a => a.DoctorId == id).Where(a => !a.IsBooked)
                .OrderBy(a => a.SlotDateTime)
                .ToList();


            // populates the appointmentViewModel with the existing doctor info 
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                ActiveDoctor = doctor,
                Availability = availableSlots,
                ActiveAppointment = new Appointment()
            };

            // return that appointmentViewModel to the view
            return View("../Healthcare/BookAppointment", appointmentViewModel);
        }


        [HttpGet("/doctor/reschedule-appointment-form")] //specifies the URL - GET handler for the blank reschedule form
        public IActionResult GetRescheduleAppointmentForm(int id)
        {
            // tries to find appointment and doctor with the id from the database
            Appointment? appointment = _healthcareDbContext.Appointment.Where(p => p.AppointmentId == id).FirstOrDefault();
            Doctor? doctor = _healthcareDbContext.Doctor.Where(p => p.DoctorId == appointment.DoctorId).FirstOrDefault();

            //gets list of available slots for that doctor
            List<Availability>? availableSlots = _healthcareDbContext.Availability.Where(a => a.DoctorId == doctor.DoctorId).Where(a => !a.IsBooked)
                .OrderBy(a => a.SlotDateTime)
                .ToList();

            // populates the appointmentViewModel with the existing appointment info and doctor
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                ActiveDoctor = doctor,
                ActiveAppointment = appointment,
                Availability = availableSlots
            };

            // return that appointmentViewModel to the view
            return View("../Healthcare/RescheduleAppointment", appointmentViewModel);
        }

        //POST handler that enables adding a appointment 
        [HttpPost("/doctor/book-appointment")]
        public IActionResult BookAppointment(AppointmentViewModel appointmentViewModel)
        {
            
            //retrieves doctor id, and patient id
            appointmentViewModel.ActiveAppointment.DoctorId = appointmentViewModel.ActiveDoctor.DoctorId;
            appointmentViewModel.ActiveAppointment.PatientId = 1; //set to 1 for now until we have different patients 

            if (ModelState.IsValid) //enter this if model state is valid 
            {
                //Get the selected slot from the database
                Availability? selectedSlot = _healthcareDbContext.Availability.Where(a => a.SlotDateTime == appointmentViewModel.ActiveAppointment.AppointmentDate).Where(a=> a.DoctorId == appointmentViewModel.ActiveDoctor.DoctorId).FirstOrDefault();

                selectedSlot.IsBooked = true; // Mark the slot as booked
                
                _healthcareDbContext.Appointment.Add(appointmentViewModel.ActiveAppointment);   // add the appointment information in the database 

                _healthcareDbContext.SaveChanges(); // saves the changes to the database
                
                // redirect to the GetAllDoctors
                return RedirectToAction("GetAllDoctors", "Healthcare");
            }
            else
            {
                // tries to find doctor with the id from the database
                Doctor? doctor = _healthcareDbContext.Doctor.Where(p => p.DoctorId == appointmentViewModel.ActiveDoctor.DoctorId).FirstOrDefault();

                //makes a list of available slots for that doctor
                List<Availability>? availableSlots = _healthcareDbContext.Availability.Where(a => a.DoctorId == appointmentViewModel.ActiveDoctor.DoctorId).Where(a => !a.IsBooked)
                    .OrderBy(a => a.SlotDateTime)
                    .ToList();


                // populates the appointmentViewModel with the existing doctor info 
                AppointmentViewModel newappointmentViewModel = new AppointmentViewModel()
                {
                    ActiveDoctor = doctor,
                    Availability = availableSlots,
                    ActiveAppointment = new Appointment()
                };

                return View("../Healthcare/BookAppointment", newappointmentViewModel);  // model is invalid so show errors and load the Add view
            }
        }

        [HttpPost("/doctor/reschedule-appointment")]
        public IActionResult RescheduleAppointment(AppointmentViewModel appointmentViewModel)
        {
            //Load the existing appointment from the database based on the id
            Appointment? existingAppointment = _healthcareDbContext.Appointment.Where(a => a.AppointmentId == appointmentViewModel.ActiveAppointment.AppointmentId).FirstOrDefault();
            
            //set the doctor id and patient id
            appointmentViewModel.ActiveAppointment.DoctorId = appointmentViewModel.ActiveDoctor.DoctorId;
            appointmentViewModel.ActiveAppointment.PatientId = 1; //set to 1 for now since we only have 1 patient


            if (ModelState.IsValid) //enter this if model state is valid 
            {
                //Load the previously selected slot from the database
                Availability? previousSlot = _healthcareDbContext.Availability
                    .FirstOrDefault(a => a.SlotDateTime == existingAppointment.AppointmentDate
                    && a.DoctorId == appointmentViewModel.ActiveDoctor.DoctorId);


                previousSlot.IsBooked = false; // Mark the slot as not booked - free it up

                //Load the currently selected slot from the database
                Availability? selectedSlot = _healthcareDbContext.Availability
                    .FirstOrDefault(a => a.SlotDateTime == appointmentViewModel.ActiveAppointment.AppointmentDate
                    && a.DoctorId == appointmentViewModel.ActiveDoctor.DoctorId);


                selectedSlot.IsBooked = true; // Mark the slot as booked

                //change the existing appointment date to the newly selected date
                existingAppointment.AppointmentDate = selectedSlot.SlotDateTime;

                _healthcareDbContext.SaveChanges(); // saves the changes to the database


                // redirect to the GetAllDoctors 
                return RedirectToAction("GetAllDoctors", "Healthcare");
            }
            else
            {
                // tries to find appointment and doctor with the id from the database
                Appointment? appointment = _healthcareDbContext.Appointment.Where(p => p.AppointmentId == appointmentViewModel.ActiveAppointment.AppointmentId).FirstOrDefault();
                Doctor? doctor = _healthcareDbContext.Doctor.Where(p => p.DoctorId == appointment.DoctorId).FirstOrDefault();

                //gets list of available slots for that doctor
                List<Availability>? availableSlots = _healthcareDbContext.Availability.Where(a => a.DoctorId == doctor.DoctorId).Where(a => !a.IsBooked)
                    .OrderBy(a => a.SlotDateTime)
                    .ToList();

                // populates the appointmentViewModel with the existing appointment info and doctor
                AppointmentViewModel newappointmentViewModel = new AppointmentViewModel()
                {
                    ActiveDoctor = doctor,
                    ActiveAppointment = appointment,
                    Availability = availableSlots
                };

                return View("../Healthcare/RescheduleAppointment", newappointmentViewModel);  // model is invalid so show errors and load the Edit view
            }
        }


        private HealthcareDbContext _healthcareDbContext; //private HealthcareDbContext variable
    }
}