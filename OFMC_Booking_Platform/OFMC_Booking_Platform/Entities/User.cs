using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required!!")]
        public string? FirstName { get; set; }


        [Required(ErrorMessage = "Last Name is required!!")]
        public string? LastName { get; set; }


        [Required(ErrorMessage = "Date of Birth is required!!")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }        // The Date and time of the event and is required.
    }
}