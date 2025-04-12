using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;

namespace OFMC_Booking_Platform.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService _adminService; //private HealthcareDbContext variable
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        // constructor - tells the DI container that this controller needs a DB context, as well access to the email and sms services
        public AdminController(IAdminService adminService, IEmailService emailService, ISmsService smsService)
        {
            this._adminService = adminService;   // references the admin service for database related operations.

            // references the notification services
            _emailService = emailService;   
            _smsService = smsService;
        }

        // GET handler for the list of all of the doctors on the admin side
        [Authorize(Roles = "Admin")]
        [HttpGet("/doctorsList")] //specifies the URL - GET handler for the list of all of the doctors

        public async Task<IActionResult> GetDoctorsList()
        {
            //retrieves a list of doctors from the database
            return View("../Admin/DoctorList", await this._adminService.GetAllDoctors());  //returns the list of doctors to the view using the view name
        }


        // Defining an action that gets all the appointments associated with a doctor and returns to the viewmodel
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId)
        {
                                                                                                      // Calling the admin service to fetch doctor and appointments from DB.
            Doctor? doctor = await this._adminService.GetDoctorByIDAndIncludeAppointments(doctorId);  // finding the doctor based on the doctor id passed to the action

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
                        AppointmentId = a.AppointmentId,
                        PatientName = a.PatientName,
                        AppointmentDate = a.AppointmentDate
                    })
                    .ToList()
            };

            return View("../Admin/DoctorAppointments", viewModel);
        }



        // defining an action that gets the information of the patient associated with an appointment 
        [Authorize(Roles = "Admin")]
        [HttpGet("/doctorAppointmentDetails")]
        public async Task<IActionResult> GetAppointmentDetails(int appointmentId)
        {
            Appointment? appointment = await this._adminService.GetAppointmentsByIDAndIncludeDoctor(appointmentId);  // find the specific appointment of the patient

            if (appointment == null)
                return NotFound();

            return View("../Admin/AppointmentDetails", appointment); // return the appointment information to the view
        }


        // Defining an action that returns the patient appointment cancel form for admin
        [Authorize(Roles = "Admin")]
        [HttpGet("/admin/cancelAppointmentForm")]
        public async Task<IActionResult> GetCancelAppointmentForm(int appointmentId)
        {

            Appointment? appointment = await this._adminService.GetAppointmentsByIDAndIncludeDoctorAndPatient(appointmentId);  // finding the appointment with the specific id passed

            if (appointment == null)
            {
                return NotFound();
            }

            AppointmentViewModel viewModel = new AppointmentViewModel   // Initializing the appointment view model 
            {
                ActiveAppointment = appointment,
                ActiveDoctor = appointment.Doctor,
                ActivePatient = appointment.Patient
            };

            return View("../Admin/CancelPatientAppointment", viewModel);   // return the viewmodel which consists of the appointment information
        }




        // Defining an action that removes the appointment of the patient from the database
        [Authorize(Roles = "Admin")]
        [HttpPost("/admin/cancelAppointment")]
        public async Task<IActionResult> CancelPatientAppointment(int appointmentId)
        {
            Appointment? appointment = await this._adminService.GetAppointmentById(appointmentId);  // find the specific appointment of the patient

            if (appointment != null)
            {


                AppointmentViewModel appointmentViewModel = new AppointmentViewModel   // Initializing the appointment view model 
                {
                    ActiveAppointment = appointment,
                    ActiveDoctor = appointment.Doctor,
                    ActivePatient = appointment.Patient
                };


                // Get the doctor from the database using AdminService to populate ActiveDoctor from the AppointmentViewModel.
                appointmentViewModel.ActiveDoctor = await this._adminService.GetDoctorById(appointmentViewModel.ActiveDoctor.DoctorId);


                // sending email and SMS notification based on contact method
                switch (appointmentViewModel.ActiveAppointment.ContactMethod)
                {

                    // Send a cancellation email if the preferred contact method is set to 'Email' by the patient when booking the appointment
                    case ContactMethod.Email:
                        _emailService.SendAdminCancellationEmail(appointmentViewModel);
                        break;

                    // Send a cancellation SMS message if the preferred contact method is set to 'Text' or 'Phone number' by the patient when booking the appointment
                    case ContactMethod.Text:
                    case ContactMethod.Phone:
                        _smsService.SendAdminCancellationSms(appointmentViewModel);
                        break;
                }



                Availability? slot = await this._adminService.GetDoctorAvailabilityBasedOnAppointmentDateAndDoctorId(appointment.AppointmentDate, appointment.DoctorId);

                if (slot != null)
                    slot.IsBooked = false;   // free up the availability of the slot for other patients once this slot is cancelled.

                await this._adminService.CancelAppointmentAndFreeSlot(appointment);

                TempData["Message"] = $"You canceled the {appointment.PatientName} appointment";
            }



            return RedirectToAction("GetDoctorAppointments", new { doctorId = appointment.DoctorId });
        }

    }
}
