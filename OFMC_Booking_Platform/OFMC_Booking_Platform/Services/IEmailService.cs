using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Services
{
    public interface IEmailService
    {

        public void SendConfirmatioEmail(AppointmentViewModel appointment);
        public void SendAdminCancellationEmail(AppointmentViewModel appointment);


    }
}
