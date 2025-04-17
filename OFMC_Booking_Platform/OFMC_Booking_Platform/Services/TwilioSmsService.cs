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




        // Defining a method that sends a confirmation SMS notification to the client once the client books an appointment
        public void SendConfirmationSms(AppointmentViewModel appointmentViewModel)
        {

            try

            {
                TwilioClient.Init(_accountSid, _authToken);

                var message = MessageResource.Create(
                    body: $"Dear {appointmentViewModel.ActiveAppointment.PatientName}, " +
                        $"your appointment with {appointmentViewModel.ActiveAppointment.Doctor.DoctorName} " +
                        $"({appointmentViewModel.ActiveDoctor.DoctorSpecialty}) is scheduled for " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("MMMM dd, yyyy")} at " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}.\n\n" +
                        $"We look forward to seeing you!\n" +
                        $"Best regards, " +
                        $"\nOakridge Family Medical Center. " +
                        $"\nPhone: (555) 987-6543 | Email: info@oakridgemedical.com",


                    messagingServiceSid: _messagingServiceSid,
                    to: new PhoneNumber(appointmentViewModel.ActiveAppointment.AppointmentPhone.Replace("-", ""))
                );
            }

            catch (Twilio.Exceptions.ApiException apiEx)
            {

                Console.WriteLine($"Twilio API Error: {apiEx.Message}");
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error for SMS services: {ex.Message}");
            }

  
        }



        // Defining a method that sends a cancellation SMS notification to the client once the admin cancels the appointment
        public void SendAdminCancellationSms(AppointmentViewModel appointmentViewModel)
        {
            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                var message = MessageResource.Create(
                    body: $"Dear {appointmentViewModel.ActiveAppointment.PatientName}, " +



                        $"we regreat to inform you that your appointment with {appointmentViewModel.ActiveAppointment.Doctor.DoctorName} " +
                        $"({appointmentViewModel.ActiveDoctor.DoctorSpecialty}) on " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("MMMM dd, yyyy")} at " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")} has been canceled.\n\n" +
                        $"We sincerely apologize for any inconvenience this may cause.\n\n" +
                        $"Best regards, " +
                        $"\nOakridge Family Medical Center. " +
                        $"\nPhone: (555) 987-6543 | Email: info@oakridgemedical.com",


                    messagingServiceSid: _messagingServiceSid,
                    to: new PhoneNumber(appointmentViewModel.ActiveAppointment.AppointmentPhone)
                );
            }


            catch (Twilio.Exceptions.ApiException apiEx)
            {

                Console.WriteLine($"Twilio API Error: {apiEx.Message}");
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error for SMS services: {ex.Message}");
            }
        }





        // Defining a method that sends a cancellation SMS notification to the patient once the admin cancels the appointment
        public void SendPatientCancellationSms(AppointmentViewModel appointmentViewModel)
        {

            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                var message = MessageResource.Create(
                    body: $"Dear {appointmentViewModel.ActiveAppointment.PatientName}, " +

                        $"you have successfully cancelled your appointment with {appointmentViewModel.ActiveAppointment.Doctor.DoctorName} " +
                        $"({appointmentViewModel.ActiveDoctor.DoctorSpecialty}) on " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("MMMM dd, yyyy")} at " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}\n\n" +

                        $"If you wish to reschedule your appointment, please visit our platform or contact us at (555) 987-6543.\n\n" +
                        $"Best regards, " +
                        $"\nOakridge Family Medical Center. " +
                        $"\nPhone: (555) 987-6543 | Email: info@oakridgemedical.com",


                    messagingServiceSid: _messagingServiceSid,
                    to: new PhoneNumber(appointmentViewModel.ActiveAppointment.AppointmentPhone)
                );
            }


            catch (Twilio.Exceptions.ApiException apiEx)
            {

                Console.WriteLine($"Twilio API Error: {apiEx.Message}");
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error for SMS services: {ex.Message}");
            }
        }




        // Defining a method that sends an SMS notification to the patient once the patient reschedules their appointment
        public void SendPatientRescheduleConfirmationSMS(AppointmentViewModel appointmentViewModel)
        {

            try
            {
                TwilioClient.Init(_accountSid, _authToken);

                var message = MessageResource.Create(
                    body: $"Dear {appointmentViewModel.ActiveAppointment.PatientName}, " +

                        $"You have successfully rescheduled your appointment with {appointmentViewModel.ActiveAppointment.Doctor.DoctorName} " +
                        $"Below are the details of your new appointment: \n" +
                        $"Doctor name: {appointmentViewModel.ActiveAppointment.Doctor.DoctorName}" +
                        $"({appointmentViewModel.ActiveDoctor.DoctorSpecialty})" +
                        $"Date and Time: {appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("MMMM dd, yyyy")} at " +
                        $"{appointmentViewModel.ActiveAppointment.AppointmentDate?.ToString("hh:mm tt")}\n\n" +

                        $"If you have any questions or need to make additional changes, feel free to call the number below or email us using the address provided.\n\n" +

                        $"Best regards, " +
                        $"\nOakridge Family Medical Center. " +
                        $"\nPhone: (555) 987-6543 | Email: info@oakridgemedical.com",


                    messagingServiceSid: _messagingServiceSid,
                    to: new PhoneNumber(appointmentViewModel.ActiveAppointment.AppointmentPhone)
                );
            }


            catch (Twilio.Exceptions.ApiException apiEx)
            {

                Console.WriteLine($"Twilio API Error: {apiEx.Message}");
            }


            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error for SMS services: {ex.Message}");
            }

        }

    }
}
