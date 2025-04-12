using OFMC_Booking_Platform.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter your First Name!!")]
        public string? FirstName { get; set; }


        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter your Last Name!!")]
        public string? LastName { get; set; }


        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is required!!")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }        // The Date and time of the event and is required.



        [RegularExpression(@"\+1[0-9]{3}-[0-9]{3}-[0-9]{4}", ErrorMessage = "Please follow the format 000-000-0000")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }



        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Address Required!!")]
        public string? Email { get; set; }



        [Required(ErrorMessage = "A Password is Required!!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }



        [ConfirmPasswordValidation]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please confirm your password!!")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
