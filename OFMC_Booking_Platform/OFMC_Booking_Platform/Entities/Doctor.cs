using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OFMC_Booking_Platform.Entities
{
    public class Doctor 
    {
        //DoctorId is the primary key
        public int DoctorId { get; set; } //can get and set the DoctorId

        public string? DoctorName { get; set; } //can get and set theDoctorName
        public string? DoctorSpecialty { get; set; } //can get and set the DoctorSpecialty
        public int? DoctorExt { get; set; } //can get and set the DoctorExt


        // need a collection of related appointments in order to get displayed on the admin side
        public ICollection<Appointment>? Appointments { get; set; } = new List<Appointment>();


    }
}
