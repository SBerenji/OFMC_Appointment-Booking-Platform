﻿using Microsoft.AspNetCore.Mvc;
using OFMC_Booking_Platform.Entities;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;
using System.IO;

namespace OFMC_Booking_Platform.Controllers
{
    public class HealthcareController : Controller //controller for the healthcare information 
    {
        // constructor - tells the DI container that this controller needs a DB context 
        public HealthcareController(HealthcareDbContext healthcareDbContext, IEmailService emailService, ISmsService smsService)
        {
            _healthcareDbContext = healthcareDbContext; //intitalizes the controller with a reference to the HealthcareDbContext
            _emailService = emailService; // injecting the email service
            _smsService = smsService;   // injecting the sms service
        }

        // GET handler for the list of all of the doctors
        [HttpGet("/doctors")] //specifies the URL - GET handler for the list of all of the doctors
        public IActionResult GetAllDoctors()
        {
            //retrieves a list of doctors from the database
            List<Doctor> doctors = _healthcareDbContext.Doctor.ToList();

            return View("../Patient/Manage", doctors);  //returns the list of doctors to the view using the view name
        }



        // GET handler for the list of all of the appointments
        [HttpGet("/appointments")] //specifies the URL - GET handler for the list of all of the appointments
        public IActionResult GetAllAppointments()
        {
            int patientId = 1;

            //retrieves a list of appointments from the database
            List<Appointment> appointments = _healthcareDbContext.Appointment
                .Include(m => m.Doctor) // Include the related Doctor data
                .Where(m => m.PatientId == patientId)
                .ToList();

            return View("../Patient/Appointments", appointments);  //returns the list of appointments to the view using the view name
        }



        [HttpGet("/doctor/book-appointment-form")] //specifies the URL - GET handler for the blank add form
        public IActionResult GetAppointmentForm(int id)
        {

            int patientID = 1;
            Patient? patient = _healthcareDbContext.Patient.Where(p => p.PatientId == patientID).FirstOrDefault();
            string patientName = patient?.FirstName + " " + patient?.LastName;


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
                ActivePatient = patient,
                Availability = availableSlots,
                ActiveAppointment = new Appointment()
                {
                    PatientName = patientName,
                    AppointmentEmail = patient?.PatientEmail
                }
            };

            // return that appointmentViewModel to the view
            return View("../Patient/BookAppointment", appointmentViewModel);
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
            return View("../Patient/RescheduleAppointment", appointmentViewModel);
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


                // Get the doctor from the database to populate ActiveDoctor from the AppointmentViewModel
                appointmentViewModel.ActiveDoctor = _healthcareDbContext.Doctor
                    .FirstOrDefault(d => d.DoctorId == appointmentViewModel.ActiveAppointment.DoctorId);

                // Send a confirmation email if the preferred contact method is set to 'Email' by the patient when booking the appointment
                if (appointmentViewModel.ActiveAppointment.ContactMethod == ContactMethod.Email) 
                {

                    _emailService.SendConfirmatioEmail(appointmentViewModel);
                }


                // Send a confirmation SMS message if the preferred contact method is set to 'Text' by the patient when booking the appointment
                // Send a confirmation email if the preferred contact method is set to 'Email' by the patient when booking the appointment
                if (appointmentViewModel.ActiveAppointment.ContactMethod == ContactMethod.Text || appointmentViewModel.ActiveAppointment.ContactMethod == ContactMethod.Phone)
                {
                    _smsService.SendConfirmationSms(appointmentViewModel);
                }

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

                return View("../Patient/BookAppointment", newappointmentViewModel);  // model is invalid so show errors and load the Add view
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

                return View("../Patient/RescheduleAppointment", newappointmentViewModel);  // model is invalid so show errors and load the Edit view
            }
        }

        /// <summary>
        /// Displays detailed info about a specific appointment.
        /// </summary>
        /// <param name="id">The appointment ID</param>
        /// <returns>The appointment info view</returns>
        [HttpGet("/appointment/info")]
        public IActionResult AppointmentInfo(int id)
        {
            var appointment = _healthcareDbContext.Appointment
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.AppointmentId == id && a.PatientId == 1); // Just temporary since we dont have login ready yet

            if (appointment == null)
                return NotFound();

            return View("../Patient/AppointmentInfo", appointment);
        }


        [HttpGet("/appointment/cancel-form")] //specifies the URL - GET handler for the blank reschedule form
        public IActionResult GetCanceltForm(int id)
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
            return View("../Patient/CancelConfirmation", appointmentViewModel);
        }

        /// <summary>
        /// Cancels the appointment and frees up the time slot.
        /// </summary>
        /// <param name="id">The appointment ID</param>
        /// <returns>Redirects to Appointments view</returns>
        [HttpPost("/appointment/cancel")]
        public IActionResult CancelAppointment(int id)
        {
            var appointment = _healthcareDbContext.Appointment
                .FirstOrDefault(a => a.AppointmentId == id && a.PatientId == 1);

            if (appointment != null)
            {
                var slot = _healthcareDbContext.Availability
                    .FirstOrDefault(s => s.SlotDateTime == appointment.AppointmentDate && s.DoctorId == appointment.DoctorId);

                if (slot != null)
                    slot.IsBooked = false;

                _healthcareDbContext.Appointment.Remove(appointment);
                _healthcareDbContext.SaveChanges();
                TempData["Message"] = "Your appointment has been cancelled.";
            }

            return RedirectToAction("GetAllAppointments");
        }

        /// <summary>
        /// Displays a confirmation page before cancelling an appointment.
        /// </summary>
        [HttpGet("/appointment/cancel-confirmation")]
        public IActionResult ConfirmCancelAppointment(int id)
        {
            var appointment = _healthcareDbContext.Appointment
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.AppointmentId == id && a.PatientId == 1);

            if (appointment == null)
                return NotFound();

            return View("../Healthcare/CancelConfirmation", appointment);
        }



        private HealthcareDbContext _healthcareDbContext; //private HealthcareDbContext variable
        private readonly IEmailService _emailService;  
        private readonly ISmsService _smsService;

    }
}