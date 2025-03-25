using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OFMC_Booking_Platform.Entities
{

    public class Availability
    {
      
        [Key]
        public int SlotId { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }  // Foreign key to Doctor

        public DateTime SlotDateTime { get; set; } // The exact date and time of the slot

        public bool IsBooked { get; set; } // Flag to mark if the slot is already booked
    }


}
