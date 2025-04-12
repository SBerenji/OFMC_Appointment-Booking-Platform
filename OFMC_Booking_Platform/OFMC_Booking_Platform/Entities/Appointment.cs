using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OFMC_Booking_Platform.Entities
{
    public enum ContactMethod { Email = 0, Phone, Text } //Enum for the Contact Method

    public class Appointment : IValidatableObject
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
        public string? PatientName { get; set; } //can get and set the PatientName

        [Required(ErrorMessage = "Please select a appointment date")] // Error message if AppointmentDate is not entered
        public DateTime? AppointmentDate { get; set; } //can get and set the AppointmentDate


        public ContactMethod ContactMethod { get; set; } = ContactMethod.Email; //ContactMethod how patient wants to be contacted

        //[Required(ErrorMessage = "Please enter a patient email")] // Error message if email is not entered
        //[EmailAddress(ErrorMessage = "Please enter a valid email")] //Error message if invalid email format is entered
        public string? AppointmentEmail { get; set; } // can get and set the email of the patient


        //[Required(ErrorMessage = "Please enter a patient phone number")] // Error message if email is not entered
        //[RegularExpression(@"^\+1\d{10}$", ErrorMessage = "Phone number must be in the format +1XXXXXXXXXX")]  // using regular expression to define the format of the phone number
        [RegularExpression(@"\+1[0-9]{3}-[0-9]{3}-[0-9]{4}", ErrorMessage = "Please follow the format 000-000-0000")]
        public string? AppointmentPhone { get; set; } // can get and set the phone number of the patient



        public string? Notes { get; set; } //can get and set the Notes

        public Doctor? Doctor { get; set; } // can get and set Doctor 



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ContactMethod == ContactMethod.Email && string.IsNullOrWhiteSpace(AppointmentEmail))
            {
                yield return new ValidationResult("Email is required when Email is the preferred contact method.", new[] { nameof(AppointmentEmail) });
            }

            if ((ContactMethod == ContactMethod.Phone || ContactMethod == ContactMethod.Text) &&
                string.IsNullOrWhiteSpace(AppointmentPhone))
            {
                yield return new ValidationResult("Phone number is required when Phone or Text is the preferred contact method.", new[] { nameof(AppointmentPhone) });
            }
        }

    }
}
