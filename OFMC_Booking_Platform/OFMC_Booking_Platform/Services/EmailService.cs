
using System.Net.Mail;
using System.Net;
using OFMC_Booking_Platform.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;  // Provides access to the configuration setting of the application (to access sensitive info like email password)

        private readonly string _fromAddress;
        private readonly string _fromPassword;


        // Constructor to initialize the email configuration
        public EmailService(IConfiguration config)
        {
            _config = config;

            // Get the email address and password of the email that will be sending the invitations
            _fromAddress = _config["EmailSettings:SenderEmail"];
            _fromPassword = _config["EmailSettings:AppPassword"];   

       }



        private readonly string _smtpHost = "smtp.gmail.com";  // The SMTP host
        private readonly int _smtpPort = 587;  // SMTP Port


        // defining the main function that sends the email by creating a new smpt client and MailMessage object
        private void SendEmail(string recipientEmail, string subject,  string body)
        {
            // Initialize the smpt client
            var smtpClient = new SmtpClient(_smtpHost)
            {
                Port = _smtpPort,
                Credentials = new NetworkCredential(_fromAddress, _fromPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
            };


            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(recipientEmail);



            try
            {
                System.Diagnostics.Debug.WriteLine("SendEmail method was called.");

                smtpClient.Send(mailMessage);  // send the email
            }

            // in case there is an issue in sending the email, log the error message
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send email to {recipientEmail}: {ex.Message}");
            }

        }


        // Defining a method that sends a confirmation email to the patient once an appointment is successfully booked by the patient
        public void SendConfirmatioEmail(AppointmentViewModel appointmentViewModel)
        {
            var body = $@"
            <p> <strong> Dear {appointmentViewModel.ActiveAppointment.PatientName} </p>
            <p style='font-size:16px;'><strong>Your appointment has been successfully scheduled.</strong></p>
            <p>Below are the details:</p>

            <p>
                <strong>Date & Time:</strong> <b>{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("dddd, MMMM d, yyyy")} at {appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}</b><br />
                <strong>Doctor:</strong> Dr. {appointmentViewModel.ActiveDoctor.DoctorName} – {appointmentViewModel.ActiveDoctor.DoctorSpecialty}<br />
                <strong>📍 Location:</strong> Oakridge Family Medical Center,<br />
                1347 Willow Creek Rd, Maplewood, ON
            </p>

            <p>We look forward to seeing you!</p>

            <p>
                Best regards,<br />
                <strong>Oakridge Family Medical Center</strong><br />
                📞 (555) 987-6543 | ✉️ info@oakridgemedical.com
            </p>";



            SendEmail(appointmentViewModel.ActiveAppointment.AppointmentEmail, "Appointment Confirmation - Oakridge Family Medical Center", body);
        }



        // Defining a method that sends a cancellation notification to the client once the admin cancels the appointment
        public void SendAdminCancellationEmail(AppointmentViewModel appointmentViewModel)
        {
            var body = $@"
            <p style='font-size:16px;'>❗ <strong>We regret to inform you that your appointment has been canceled.</strong></p>

            <p>
                Your appointment with Dr. {appointmentViewModel.ActiveDoctor.DoctorName} on 
                {appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("dd, MM, yyyy")} at {appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}
                has been canceled. </b>
            </p>

            <p>We sincerely apologize for any inconvenience this may cause.</p>

            <p>
                Please contact us at (555) 987-6543<br />
                or book a new appointment through the Oakridge Family Medical Center Booking Platform to reschedule.
            </p>

            <p>Thank you for your understanding.</p>

            <p>
                Best regards,<br />
                <strong>Oakridge Family Medical Center</strong><br />
                📞 (555) 987-6543 | ✉️ info@oakridgemedical.com
            </p>";

            SendEmail(appointmentViewModel.ActiveAppointment.AppointmentEmail, "Appointment Cancellation - Oakridge Family Medical Center", body);

        }





    }
}
