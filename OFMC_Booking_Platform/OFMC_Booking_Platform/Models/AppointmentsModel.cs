using OFMC_Booking_Platform.Entities;
using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.Models
{
    public class AppointmentsModel //a model that holds data that is send from controller to view
    {
        public Patient? Patient { get; set; }

        public List<Appointment>? Appointments { get; set; } = new List<Appointment>();

    }
}
