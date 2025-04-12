using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Services
{
    public interface IEmailService
    {

        public void SendConfirmatioEmail(AppointmentViewModel appointment);
        public void SendAdminCancellationEmail(AppointmentViewModel appointment);
        public void SendPatientCancellationEmail(AppointmentViewModel appointmentViewModel);
        public void SendPatientRescheduleConfirmationEmail(AppointmentViewModel appointmentViewModel);

    }
}
