using OFMC_Booking_Platform.Models;
using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.CustomValidations
{
    public class ConfirmPasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            RegisterViewModel instanceRegisterViewModel = (RegisterViewModel)validationContext.ObjectInstance;

            if (instanceRegisterViewModel.Password != instanceRegisterViewModel.ConfirmPassword)
            {
                return new ValidationResult("Password and Confirm Password fields must match.");
            };

            return ValidationResult.Success;
        }
    }
}
