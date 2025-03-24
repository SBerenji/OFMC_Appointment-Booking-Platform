using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Models
{
    public class AppointmentsViewModel //a model that holds data that is send from controller to view
    {

        List<Appointment>? Appointments { get; set; }

        public string? DoctorName { get; set; }
        public string? DoctorSpecialty {  get; set; }

    }
}
