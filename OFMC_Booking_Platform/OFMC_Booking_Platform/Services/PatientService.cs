using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Services
{
    public class PatientService : IPatientService
    {
        private readonly HealthcareDbContext _healthcareDbContext;

        public PatientService(HealthcareDbContext healthcareDbContext) {
            this._healthcareDbContext = healthcareDbContext;
        }

        public async Task<List<Doctor>> GetAllDoctors()
        {
            return await this._healthcareDbContext.Doctor.ToListAsync();
        }

        public async Task<Patient?> GetPatientByClaimsUserId(string UserId)
        {
            return await _healthcareDbContext.Patient
                .FirstOrDefaultAsync(p => p.UserId == UserId);
        }


        public async Task<List<Appointment>> GetListOfAppointmentsAndIncludeDoctorsBasedOnPatientId(int PatientId)
        {
            return await this._healthcareDbContext.Appointment
                .Include(m => m.Doctor) // Include the related Doctor data
                .Where(a => a.AppointmentDate > DateTime.Now && a.PatientId == PatientId)
                .ToListAsync();
        }


        public async Task<List<Appointment>> GetListOfPastAppointmentsAndIncludeDoctorsBasedOnCurrentTimeAndPatientId(int PatientId)
        {
            return await this._healthcareDbContext.Appointment
                .Include(a => a.Doctor)
                .Where(a => a.AppointmentDate < DateTime.Now && a.PatientId == PatientId) // mock for Sara
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }


        public async Task<Doctor?> GetDoctorById(int DoctorId)
        {
            return await this._healthcareDbContext.Doctor.Where(p => p.DoctorId == DoctorId).FirstOrDefaultAsync();
        }


        public async Task<List<Availability>?> GetAllAvailableAppointmentSlotsForADoctorBasedOnDoctorId(int DoctorId)
        {
            //gets list of available slots for a doctor.
            return await this._healthcareDbContext.Availability.Where(a => a.DoctorId == DoctorId).Where(a => !a.IsBooked)
                .OrderBy(a => a.SlotDateTime)
                .ToListAsync();
        }


        public async Task<Appointment?> GetAppointmentById(int AppointmentId)
        {
            return await this._healthcareDbContext.Appointment.Where(p => p.AppointmentId == AppointmentId).FirstOrDefaultAsync();
        }


        public async Task<Availability?> GetTheSelectedSlotBasedOnAppointmentDateAndDoctorId(DateTime? AppointmentDate, int DoctorId)
        {
            return await this._healthcareDbContext.Availability
                .Where(a => a.SlotDateTime == AppointmentDate)
                .Where(a => a.DoctorId == DoctorId)
                .FirstOrDefaultAsync();
        }


        public async Task AddAnAppointmentAndBookSlot(Appointment ActiveAppointment)
        {
            this._healthcareDbContext.Appointment.Add(ActiveAppointment);   // add the appointment information in the database 

            await this._healthcareDbContext.SaveChangesAsync(); // saves the changes to the database
        }

        public async Task SaveChangesInDB()
        {
            await this._healthcareDbContext.SaveChangesAsync(); // saves the changes to the database
        }


        public async Task<Appointment?> GetAnAppointmentAndIncludeDoctorBasedOnAppointmentIdAndPatientId(int AppointmentId, int PatientId)
        {
            return await this._healthcareDbContext.Appointment
                   .Include(a => a.Doctor)
                   .FirstOrDefaultAsync(a => a.AppointmentId == AppointmentId && a.PatientId == PatientId);
        }


        public async Task<Appointment?> GetAnAppointmentBasedOnAppointmentIdAndPatientId(int AppointmentId, int PatientId)
        {
            return await this._healthcareDbContext.Appointment
                .FirstOrDefaultAsync(a => a.AppointmentId == AppointmentId && a.PatientId == PatientId);
        }


        public async Task CancelAppointmentAndFreeSlot(Appointment appointment)
        {
            this._healthcareDbContext.Appointment.Remove(appointment);

            await this._healthcareDbContext.SaveChangesAsync();
        }
    }
}
