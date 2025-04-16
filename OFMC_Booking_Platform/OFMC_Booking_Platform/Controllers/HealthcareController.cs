using Microsoft.AspNetCore.Mvc;
using OFMC_Booking_Platform.Entities;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace OFMC_Booking_Platform.Controllers
{
    public class HealthcareController : Controller //controller for the healthcare information 
    {
        // constructor - tells the DI container that this controller needs a DB context 
        public HealthcareController(IPatientService patientService, IEmailService emailService, ISmsService smsService)
        {
            this._patientService = patientService; // Injecting the Patient Service for DB operations.
            _emailService = emailService; // injecting the email service
            _smsService = smsService;   // injecting the sms service
        }


        [Authorize(Roles = "Patient")]
        // GET handler for the list of all of the doctors
        [HttpGet("/doctors")] //specifies the URL - GET handler for the list of all of the doctors
        public async Task<IActionResult> GetAllDoctors()
        {
            //retrieves a list of doctors from the database using PatientService interface.
            return View("../Patient/Manage", await this._patientService.GetAllDoctors());  //returns the list of doctors to the view using the view name
        }



        // GET handler for the list of all of the appointments
        [Authorize(Roles = "Patient")]
        [HttpGet("/appointments")] //specifies the URL - GET handler for the list of all of the appointments
        public async Task<IActionResult> GetAllAppointments()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(); 
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            //retrieves a list of appointments from the database

            List<Appointment> appointments = await this._patientService.GetListOfAppointmentsAndIncludeDoctorsBasedOnPatientId(patient.PatientId);

            AppointmentsModel appointmentsmodel = new AppointmentsModel()
            {
                Patient = patient,
                Appointments = appointments
            };

            return View("../Patient/Appointments", appointmentsmodel);  //returns the list of appointments to the view using the view name
        }

        [HttpGet("/appointments/history")]
        public async Task<IActionResult> GetAppointmentHistory()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            return View("../Patient/AppointmentHistory", await this._patientService.GetListOfPastAppointmentsAndIncludeDoctorsBasedOnCurrentTimeAndPatientId(patient.PatientId));
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("/doctor/book-appointment-form")] //specifies the URL - GET handler for the blank add form
        public async Task<IActionResult> GetAppointmentForm(int id)
        {

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(); 
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            // Patient? patient = _healthcareDbContext.Patient.Where(p => p.PatientId == patientID).FirstOrDefault();
            string patientName = patient?.FirstName + " " + patient?.LastName;


            // tries to find doctor with the id from the database
            Doctor? doctor = await this._patientService.GetDoctorById(id);

            //makes a list of available slots for that doctor
            List<Availability>? availableSlots = await this._patientService.GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(id);

            // populates the appointmentViewModel with the existing doctor info 
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                ActiveDoctor = doctor,
                ActivePatient = patient,
                Availability = availableSlots,
                ActiveAppointment = new Appointment()
                {
                    PatientName = patientName,
                    AppointmentEmail = patient?.PatientEmail,
                    AppointmentPhone = patient?.PatientPhone
                }
            };

            // return that appointmentViewModel to the view
            return View("../Patient/BookAppointment", appointmentViewModel);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("/doctor/reschedule-appointment-form")] //specifies the URL - GET handler for the blank reschedule form
        public async Task<IActionResult> GetRescheduleAppointmentForm(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(); 
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            // Patient? patient = _healthcareDbContext.Patient.Where(p => p.PatientId == patientID).FirstOrDefault();
            string patientName = patient?.FirstName + " " + patient?.LastName;



            // tries to find appointment and doctor with the id from the database
            Appointment? appointment = await this._patientService.GetAppointmentById(id);
            Doctor? doctor = await this._patientService.GetDoctorById(appointment.DoctorId);

            //gets list of available slots for that doctor
            List<Availability>? availableSlots = await this._patientService.GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(doctor.DoctorId);

            // populates the appointmentViewModel with the existing appointment info and doctor
            AppointmentViewModel appointmentViewModel = new AppointmentViewModel()
            {
                ActivePatient = patient,
                ActiveDoctor = doctor,
                ActiveAppointment = appointment,
                Availability = availableSlots
            };

            // return that appointmentViewModel to the view
            return View("../Patient/RescheduleAppointment", appointmentViewModel);
        }


        //POST handler that enables adding a appointment 
        [Authorize(Roles = "Patient")]
        [HttpPost("/doctor/book-appointment")]
        public async Task<IActionResult> BookAppointment(AppointmentViewModel appointmentViewModel)
        {

            //retrieves doctor id, and patient id
            appointmentViewModel.ActiveAppointment.DoctorId = appointmentViewModel.ActiveDoctor.DoctorId;
            appointmentViewModel.ActiveAppointment.PatientId = appointmentViewModel.ActivePatient.PatientId; //set to 1 for now until we have different patients 

            if (ModelState.IsValid) //enter this if model state is valid 
            {
                //Get the selected slot from the database
                Availability? selectedSlot = await this._patientService.GetTheSelectedSlotBasedOnAppointmentDateAndDoctorId(appointmentViewModel.ActiveAppointment.AppointmentDate, appointmentViewModel.ActiveDoctor.DoctorId);

                selectedSlot.IsBooked = true; // Mark the slot as booked

                // add the appointment information in the database 
                // saves the changes to the database
                await this._patientService.AddAnAppointmentAndBookSlot(appointmentViewModel.ActiveAppointment);


                // Get the doctor from the database to populate ActiveDoctor from the AppointmentViewModel
                appointmentViewModel.ActiveAppointment.Doctor = await this._patientService.GetDoctorById(appointmentViewModel.ActiveAppointment.DoctorId);
                //appointmentViewModel.ActiveDoctor = _healthcareDbContext.Doctor
                //    .FirstOrDefault(d => d.DoctorId == appointmentViewModel.ActiveAppointment.DoctorId);



                // sending email and SMS notification based on contact method
                switch (appointmentViewModel.ActiveAppointment.ContactMethod)
                {

                    // Send a confirmation email if the preferred contact method is set to 'Email' by the patient when booking the appointment
                    case ContactMethod.Email:
                        _emailService.SendConfirmatioEmail(appointmentViewModel);
                        break;

                    // Send a confirmation SMS message if the preferred contact method is set to 'Text' by the patient when booking the appointment
                    case ContactMethod.Text:
                        _smsService.SendConfirmationSms(appointmentViewModel);
                        break;
                }



                // redirect to the GetAllDoctors
                return RedirectToAction("GetAllAppointments", "Healthcare");

            }
            else
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized(); 
                }

                Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

                if (patient == null)
                {
                    return NotFound("Patient not found");
                }

                // Patient? patient = _healthcareDbContext.Patient.Where(p => p.PatientId == patientID).FirstOrDefault();
                string patientName = patient?.FirstName + " " + patient?.LastName;


                // tries to find doctor with the id from the database
                Doctor? doctor = await this._patientService.GetDoctorById(appointmentViewModel.ActiveDoctor.DoctorId);

                //makes a list of available slots for that doctor
                List<Availability>? availableSlots = await this._patientService.GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(appointmentViewModel.ActiveDoctor.DoctorId);

                // populates the appointmentViewModel with the existing doctor info 
                AppointmentViewModel appointmentViewModelNew = new AppointmentViewModel()
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
                return View("../Patient/BookAppointment", appointmentViewModelNew);
            }
        }



        // Am action that enables patients to reschedule their appointment by freeing up the previous timeslot and booking a new one
        [Authorize(Roles = "Patient")]
        [HttpPost("/doctor/reschedule-appointment")]
        public async Task<IActionResult> RescheduleAppointment(AppointmentViewModel appointmentViewModel)
        {
            //Load the existing appointment from the database based on the id
            Appointment? existingAppointment = await this._patientService.GetAppointmentById(appointmentViewModel.ActiveAppointment.AppointmentId);

            //set the doctor id and patient id
            appointmentViewModel.ActiveAppointment.DoctorId = appointmentViewModel.ActiveDoctor.DoctorId;
            appointmentViewModel.ActiveAppointment.PatientId = appointmentViewModel.ActivePatient.PatientId; //set to 1 for now until we have different patients 


            if (ModelState.IsValid) //enter this if model state is valid 
            {
                //Load the previously selected slot from the database
                Availability? previousSlot = await this._patientService.GetTheSelectedSlotBasedOnAppointmentDateAndDoctorId(existingAppointment.AppointmentDate, appointmentViewModel.ActiveDoctor.DoctorId);


                previousSlot.IsBooked = false; // Mark the slot as not booked - free it up

                //Load the currently selected slot from the database
                Availability? selectedSlot = await this._patientService.GetTheSelectedSlotBasedOnAppointmentDateAndDoctorId(appointmentViewModel.ActiveAppointment.AppointmentDate, appointmentViewModel.ActiveDoctor.DoctorId);


                selectedSlot.IsBooked = true; // Mark the slot as booked

                //change the existing appointment date to the newly selected date
                existingAppointment.AppointmentDate = selectedSlot.SlotDateTime;

                await this._patientService.SaveChangesInDB(); // saves the changes to the database


                // Get the doctor from the database to populate ActiveDoctor from the AppointmentViewModel
                //appointmentViewModel.ActiveDoctor = await this._patientService.GetDoctorById(appointmentViewModel.ActiveAppointment.DoctorId);
                appointmentViewModel.ActiveAppointment.Doctor = await this._patientService.GetDoctorById(appointmentViewModel.ActiveAppointment.DoctorId);

                // sending email and SMS notification based on contact method
                switch (appointmentViewModel.ActiveAppointment.ContactMethod)
                {

                    // Send a confirmation email for rescheduling if the preferred contact method is set to 'Email' by the patient when booking the appointment
                    case ContactMethod.Email:
                        _emailService.SendPatientRescheduleConfirmationEmail(appointmentViewModel);
                        break;

                    // Send a confirmation SMS message for rescheduling  if the preferred contact method is set to 'Text' by the patient when booking the appointment
                    case ContactMethod.Text:
                        _smsService.SendPatientRescheduleConfirmationSMS(appointmentViewModel);
                        break;
                }


                // redirect to the GetAllDoctors 
                return RedirectToAction("GetAllAppointments", "Healthcare");
            }
            else
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized();
                }

                Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

                if (patient == null)
                {
                    return NotFound("Patient not found");
                }

                // Patient? patient = _healthcareDbContext.Patient.Where(p => p.PatientId == patientID).FirstOrDefault();
                string patientName = patient?.FirstName + " " + patient?.LastName;



                // tries to find appointment and doctor with the id from the database
                Appointment? appointment = await this._patientService.GetAppointmentById(appointmentViewModel.ActiveAppointment.AppointmentId);
                Doctor? doctor = await this._patientService.GetDoctorById(appointment.DoctorId);

                //gets list of available slots for that doctor
                List<Availability>? availableSlots = await this._patientService.GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(doctor.DoctorId);

                // populates the appointmentViewModel with the existing appointment info and doctor
                AppointmentViewModel appointmentViewModelNew = new AppointmentViewModel()
                {
                    ActivePatient = patient,
                    ActiveDoctor = doctor,
                    ActiveAppointment = appointment,
                    Availability = availableSlots
                };

                // return that appointmentViewModel to the view
                return View("../Patient/RescheduleAppointment", appointmentViewModel);
            }
        }

        /// <summary>
        /// Displays detailed info about a specific appointment.
        /// </summary>
        /// <param name="id">The appointment ID</param>
        /// <returns>The appointment info view</returns>
        [Authorize(Roles = "Patient")]
        [HttpGet("/appointment/info")]
        public async Task<IActionResult> AppointmentInfo(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(); 
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            
            Appointment? appointment = await this._patientService.GetAnAppointmentAndIncludeDoctorBasedOnAppointmentIdAndPatientId(id, patient.PatientId); // Just temporary since we don't have login ready yet

            if (appointment == null)
                return NotFound();

            return View("../Patient/AppointmentInfo", appointment);
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("/appointment/cancel-form")] //specifies the URL - GET handler for the blank reschedule form
        public async Task<IActionResult> GetCancelForm(int id)
        {
            // tries to find appointment and doctor with the id from the database
            Appointment? appointment = await this._patientService.GetAppointmentById(id);
            Doctor? doctor = await this._patientService.GetDoctorById(appointment.DoctorId);

            //gets list of available slots for that doctor
            List<Availability>? availableSlots = await this._patientService.GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(doctor.DoctorId);

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
        [Authorize(Roles = "Patient")]
        [HttpPost("/appointment/cancel")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            
            Appointment? appointment = await this._patientService.GetAnAppointmentBasedOnAppointmentIdAndPatientId(id, patient.PatientId);

            if (appointment != null)
            {

                AppointmentViewModel appointmentViewModel = new AppointmentViewModel   // Initializing the appointment view model 
                {
                    ActiveAppointment = appointment,
                    ActiveDoctor = appointment.Doctor,
                    ActivePatient = appointment.Patient
                };


                // Get the doctor from the database to populate ActiveDoctor from the AppointmentViewModel
                appointmentViewModel.ActiveDoctor = await this._patientService.GetDoctorById(appointmentViewModel.ActiveAppointment.DoctorId);
                appointmentViewModel.ActiveAppointment.Doctor = await this._patientService.GetDoctorById(appointmentViewModel.ActiveAppointment.DoctorId);



                // sending email and SMS notification based on contact method
                switch (appointmentViewModel.ActiveAppointment.ContactMethod)
                {

                    // Send a cancellation email if the preferred contact method is set to 'Email' by the patient when booking the appointment
                    case ContactMethod.Email:
                        _emailService.SendPatientCancellationEmail(appointmentViewModel);
                        break;

                    // Send a cancellation SMS message if the preferred contact method is set to 'Text' by the patient when booking the appointment
                    case ContactMethod.Text:
                        _smsService.SendPatientCancellationSms(appointmentViewModel);
                        break;
                }

            
                Availability? slot = await this._patientService.GetTheSelectedSlotBasedOnAppointmentDateAndDoctorId(appointment.AppointmentDate, appointment.DoctorId);

                if (slot != null)
                    slot.IsBooked = false;

                await this._patientService.CancelAppointmentAndFreeSlot(appointment);

                TempData["Message"] = "Your appointment has been cancelled.";
            }

            return RedirectToAction("GetAllAppointments");
        }

        /// <summary>
        /// Displays a confirmation page before cancelling an appointment.
        /// </summary>
        [Authorize(Roles = "Patient")]
        [HttpGet("/appointment/cancel-confirmation")]
        public async Task<IActionResult> ConfirmCancelAppointment(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized(); 
            }

            Patient? patient = await this._patientService.GetPatientByClaimsUserId(userId);

            if (patient == null)
            {
                return NotFound("Patient not found");
            }

            
            Appointment? appointment = await this._patientService.GetAnAppointmentAndIncludeDoctorBasedOnAppointmentIdAndPatientId(id, patient.PatientId);

            if (appointment == null)
                return NotFound();

            return View("../Healthcare/CancelConfirmation", appointment);
        }


        private readonly IPatientService _patientService; //private HealthcareDbContext variable
        private readonly IEmailService _emailService;  
        private readonly ISmsService _smsService;

    }
}