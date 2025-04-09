using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;
using OFMC_Booking_Platform.Services;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
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


            TwilioClient.Init(_accountSid, _authToken);

            var message = MessageResource.Create(
                body: $"Hi {appointmentViewModel.ActiveAppointment.PatientName}, your appointment is scheduled for {appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("dd, MM, yyyy")} at {appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}. - Oakridge Family Medical Center",
                messagingServiceSid: _messagingServiceSid,
                to: new PhoneNumber(appointmentViewModel.ActiveAppointment.AppointmentPhone)
            );




        }

    }
}
