using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email Address Required!!")]
        public string? Email { get; set; }



        [Required(ErrorMessage = "A Password is Required!!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}