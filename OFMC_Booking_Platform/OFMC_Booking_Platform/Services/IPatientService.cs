using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Services
{
    public interface IPatientService
    {
        Task<List<Doctor>> GetAllDoctors();

        Task<Patient?> GetPatientByClaimsUserId(string UserId);

        Task<List<Appointment>> GetListOfAppointmentsAndIncludeDoctorsBasedOnPatientId(int PatientId);

        Task<List<Appointment>> GetListOfPastAppointmentsAndIncludeDoctorsBasedOnCurrentTimeAndPatientId(int PatientId);

        Task<Doctor?> GetDoctorById(int DoctorId);

        Task<List<Availability>?> GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(int DoctorId);

        Task<Appointment?> GetAppointmentById(int AppointmentId);

        Task<Availability?> GetTheSelectedSlotBasedOnAppointmentDateAndDoctorId(DateTime? AppointmentDate, int DoctorId);

        Task AddAnAppointmentAndBookSlot(Appointment appointment);

        Task SaveChangesInDB();

        Task<Appointment?> GetAnAppointmentAndIncludeDoctorBasedOnAppointmentIdAndPatientId(int AppointmentId, int PatientId);

        Task<Appointment?> GetAnAppointmentBasedOnAppointmentIdAndPatientId(int AppointmentId, int PatientId);

        Task CancelAppointmentAndFreeSlot(Appointment appointment);
    }
}
