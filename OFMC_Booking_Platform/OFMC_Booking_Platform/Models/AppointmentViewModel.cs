using OFMC_Booking_Platform.Entities;
using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.Models
{
    public class AppointmentViewModel //a model that holds data that is send from controller to view
    {
        public Doctor? ActiveDoctor { get; set; }

        [Required]
        public Appointment? ActiveAppointment { get; set; }

        public Patient? ActivePatient { get; set; }

        public List<Availability>? Availability { get; set; } = new List<Availability>();

    }
}
