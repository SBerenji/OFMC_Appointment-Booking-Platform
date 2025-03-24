using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Models
{
    public class AppointmentViewModel //a model that holds data that is send from controller to view
    {
        public Doctor? ActiveDoctor { get; set; } 

        public Appointment? ActiveAppointment { get; set; }

        public Patient? ActivePatient { get; set; }

    }
}
