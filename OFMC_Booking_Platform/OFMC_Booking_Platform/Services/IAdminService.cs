using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Services
{
    public interface IAdminService
    {
        Task<List<Doctor>> GetAllDoctors();

        Task<Doctor?> GetDoctorByIDAndIncludeAppointments(int doctorId);

        Task<Appointment?> GetAppointmentsByIDAndIncludeDoctor(int appointmentId);

        Task<Appointment?> GetAppointmentsByIDAndIncludeDoctorAndPatient(int appointmentId);

        Task<Appointment?> GetAppointmentById(int appointmentId);

        Task<Doctor?> GetDoctorById(int doctorId);

        Task<Availability?> GetDoctorAvailabilityBasedOnAppointmentDateAndDoctorId(DateTime? appointmentDate, int doctorId);

        Task CancelAppointmentAndFreeSlot(Appointment appointment);
    }
}
