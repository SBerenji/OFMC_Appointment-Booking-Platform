using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;

namespace OFMC_Booking_Platform.Controllers
{
    public class AdminController : Controller
    {

        private HealthcareDbContext _healthcareDbContext; //private HealthcareDbContext variable
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;

        // constructor - tells the DI container that this controller needs a DB context, as well access to the email and sms services
        public AdminController(HealthcareDbContext healthcareDbContext, IEmailService emailService, ISmsService smsService)
        {
            _healthcareDbContext = healthcareDbContext; //intitalizes the controller with a reference to the HealthcareDbContext

            // references the notification services
            _emailService = emailService;   
            _smsService = smsService;
        }

        // GET handler for the list of all of the doctors on the admin side
        [HttpGet("/doctorsList")] //specifies the URL - GET handler for the list of all of the doctors
        public IActionResult GetDoctorsList()
        {
            //retrieves a list of doctors from the database
            List<Doctor> doctors = _healthcareDbContext.Doctor.ToList();

            return View("../Admin/DoctorList", doctors);  //returns the list of doctors to the view using the view name
        }


        // Defining an action that gets all the appointments associated with a doctor and returns to the viewmodel
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
                        AppointmentId = a.AppointmentId,
                        PatientName = a.PatientName,
                        AppointmentDate = a.AppointmentDate
                    })
                    .ToList()
            };

            return View("../Admin/DoctorAppointments", viewModel);
        }



        // defining an action that gets the information of the patient associated with an appointment 
        [HttpGet("/doctorAppointmentDetails")]
        public IActionResult GetAppointmentDetails(int appointmentId)
        {
            Appointment appointment = _healthcareDbContext.Appointment
                 .Include(a => a.Doctor)
                 .FirstOrDefault(a => a.AppointmentId == appointmentId);  // find the specific appointment of the patient

            if (appointment == null)
                return NotFound();

            return View("../Admin/AppointmentDetails", appointment); // return the appointment information to the view
        }


        // Defining an action that returns the patient appointment cancel form for admin
        [HttpGet("/admin/cancelAppointmentForm")]
        public IActionResult GetCancelAppointmentForm(int appointmentId)
        {

            Appointment appointment = _healthcareDbContext.Appointment  // finding the appointment with the specific id passed
             .Include(a => a.Doctor)
             .Include(a => a.Patient)
             .FirstOrDefault(a => a.AppointmentId == appointmentId);

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
        [HttpPost("/admin/cancelAppointment")]
        public IActionResult CancelPatientAppointment(int appointmentId)
        {
            var appointment = _healthcareDbContext.Appointment
            .FirstOrDefault(a => a.AppointmentId == appointmentId); // find the specific appointment of the patient

            if (appointment != null)
            {


                AppointmentViewModel appointmentViewModel = new AppointmentViewModel   // Initializing the appointment view model 
                {
                    ActiveAppointment = appointment,
                    ActiveDoctor = appointment.Doctor,
                    ActivePatient = appointment.Patient
                };


                // Get the doctor from the database to populate ActiveDoctor from the AppointmentViewModel
                appointmentViewModel.ActiveDoctor = _healthcareDbContext.Doctor
                    .FirstOrDefault(d => d.DoctorId == appointmentViewModel.ActiveAppointment.DoctorId);


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




                //// Send a cancellation email if the preferred contact method is set to 'Email' by the patient when booking the appointment
                //if (appointment.ContactMethod == ContactMethod.Email)
                //{

                //    _emailService.SendAdminCancellationEmail(appointmentViewModel);
                //}


                //// Send a cancellation SMS message if the preferred contact method is set to 'Text' by the patient when booking the appointment
                //if (appointment.ContactMethod == ContactMethod.Text || appointment.ContactMethod == ContactMethod.Phone)
                //{
                //    _smsService.SendAdminCancellationSms(appointmentViewModel);
                //}




                Availability slot = _healthcareDbContext.Availability
                    .FirstOrDefault(s => s.SlotDateTime == appointment.AppointmentDate && s.DoctorId == appointment.DoctorId);

                if (slot != null)
                    slot.IsBooked = false;   // free up the availability of the slot for other patients once this slot is canceleted

                _healthcareDbContext.Appointment.Remove(appointment);
                _healthcareDbContext.SaveChanges();

                TempData["Message"] = $"You canceled the {appointment.PatientName} appointment";
            }



            return RedirectToAction("GetDoctorAppointments", new { doctorId = appointment.DoctorId });
        }

    }
}
