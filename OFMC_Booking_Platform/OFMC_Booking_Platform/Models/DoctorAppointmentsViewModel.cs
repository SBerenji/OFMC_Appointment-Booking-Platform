﻿using OFMC_Booking_Platform.Entities;
using System.ComponentModel.DataAnnotations;

namespace OFMC_Booking_Platform.Models
{
    public class DoctorAppointmentsViewModel //a model that holds data to display the appointments associated with a doctor
    {

        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public List<AppointmentInfoViewModel> Appointments { get; set; } = new List<AppointmentInfoViewModel>();


    }
}
