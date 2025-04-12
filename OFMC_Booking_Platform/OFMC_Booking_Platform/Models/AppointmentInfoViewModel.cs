namespace OFMC_Booking_Platform.Models
{
    public class AppointmentInfoViewModel
    {
        public string? PatientName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentTime => AppointmentDate?.ToString("hh:mm tt");
    }
}
