using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;


namespace OFMC_Booking_Platform.Services
{

    // Twilio Nuget package is required for Twilio to work
    public class TwilioSmsService : ISmsService
    {

        private readonly IConfiguration _config;  // Provides access to the configuration setting of the application (to access sensitive info like email password)



        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _twilioPhoneNumber;
        private readonly string _messagingServiceSid;


        public TwilioSmsService(IConfiguration config)
        {
            _config = config;

            // Get the email address and password of the email that will be sending the invitations
            _accountSid = _config["Twilio:AccountSid"];
            _authToken = _config["Twilio:AuthToken"];
            _twilioPhoneNumber = _config["Twilio:PhoneNumber"];
            _messagingServiceSid = _config["Twilio:MessagingServiceSID"];
        }




        public void SendConfirmationSms(AppointmentViewModel appointmentViewModel)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var message = MessageResource.Create(
                body: $"Hi {appointmentViewModel.ActiveAppointment.PatientName}, " +
                    $"your appointment with {appointmentViewModel.ActiveAppointment.Doctor.DoctorName} " +
                    $"({appointmentViewModel.ActiveDoctor.DoctorSpecialty}) is scheduled for " +
                    $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("MMMM dd, yyyy")} at " +
                    $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}.\n\n" +
                    $"Best regards, " +
                    $"\nOakridge Family Medical Center. " +
                    $"\nPhone: (555) 987-6543 | Email: info@oakridgemedical.com",


                messagingServiceSid: _messagingServiceSid,
                to: new PhoneNumber(appointmentViewModel.ActiveAppointment.AppointmentPhone.Replace("-", ""))
            );




        }

    }
}
