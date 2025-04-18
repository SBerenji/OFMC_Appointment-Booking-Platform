﻿using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OFMC_Booking_Platform.Entities
{
    public class Patient 
    {
        public string UserId { get; set; }

        //PatientId is the primary key
        public int PatientId { get; set; } //can get and set the PatientId


        [Required(ErrorMessage = "Please enter a firstname")] // Error message if FirstName is not entered
        public string? FirstName { get; set; } //can get and set the FirstName

        [Required(ErrorMessage = "Please enter a lastname")] // Error message if LastName is not entered
        public string? LastName { get; set; } //can get and set the LastName

        [Required(ErrorMessage = "Please enter a date of birth")] // Error message if DOB is not entered
        public DateTime? DOB { get; set; } //can get and set the DOB

        [Required(ErrorMessage = "Please enter a email address")] // Error message if email is not entered
        [EmailAddress(ErrorMessage = "Please enter a valid email")] //Error message if invalid email format is entered
        public string? PatientEmail { get; set; } // can get and set the PatientEmail

        public string? PatientPhone { get; set; } // can get and set the phone number of the patient


    }
}

