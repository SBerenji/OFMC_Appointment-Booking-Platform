using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Services
{
    public interface ISmsService
    {
        void SendConfirmationSms(AppointmentViewModel appointment);

        void SendAdminCancellationSms(AppointmentViewModel appointmentViewModel);

        public void SendPatientCancellationSms(AppointmentViewModel appointmentViewModel);

        public void SendPatientRescheduleConfirmationSMS(AppointmentViewModel appointmentViewModel);

    }
}
