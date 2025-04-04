using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OFMC_Booking_Platform.Entities
{
    public enum ContactMethod { Email = 0, Phone, Text } //Enum for the Invite Status

    public class Appointment 
    {
        [Key]
        //AppointmentId is the primary key
        public int AppointmentId { get; set; } //can get and set the AppointmentId

        [Required] //DoctorId is a required field
        [ForeignKey("Doctor")] // DoctorId is the foreign key
        public int DoctorId { get; set; } // can get and set the DoctorId 

        [Required] //PatientId is a required field
        [ForeignKey("Patient")] // PatientId is the foreign key
        public int PatientId { get; set; } // can get and set the PatientId


        [Required(ErrorMessage = "Please enter a patient name")] // Error message if PatientName is not entered
        public string? PatientName { get; set; } //can get and set the party PatientName

        [Required(ErrorMessage = "Please select a appointment date")] // Error message if AppointmentDate is not entered
        public DateTime? AppointmentDate { get; set; } //can get and set the AppointmentDate


        public ContactMethod ContactMethod { get; set; } = ContactMethod.Email; //ContactMethod how patient wants to be contacted

        [Required(ErrorMessage = "Please enter a patient email")] // Error message if email is not entered
        [EmailAddress(ErrorMessage = "Please enter a valid email")] //Error message if invalid email format is entered
        public string AppointmentEmail { get; set; } // can get and set the email of the patient


        public string? Notes { get; set; } //can get and set the Notes

        public Doctor? Doctor { get; set; } // can get and set Doctor 


    }
}
