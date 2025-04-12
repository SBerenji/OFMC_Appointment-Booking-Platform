using Microsoft.EntityFrameworkCore;
using OFMC_Booking_Platform.Entities;
using OFMC_Booking_Platform.Models;

namespace OFMC_Booking_Platform.Services
{
    public class AdminService : IAdminService
    {
        private readonly HealthcareDbContext _healthcareDbContext;

        public AdminService(HealthcareDbContext healthcareDbContext)
        {
            this._healthcareDbContext = healthcareDbContext;
        }

        public async Task<List<Doctor>> GetAllDoctors()
        {
            return await this._healthcareDbContext.Doctor.ToListAsync();
        }


        public async Task<Doctor?> GetDoctorByIDAndIncludeAppointments(int doctorId)
        {
            Doctor? doctor = await this._healthcareDbContext.Doctor
                .Include(d => d.Appointments)
                .FirstOrDefaultAsync(d => d.DoctorId == doctorId);

            if (doctor == null)
            {
                return null;
            };

            return doctor;
        }


        public async Task<Appointment?> GetAppointmentsByIDAndIncludeDoctor(int appointmentId)
        {
            Appointment? appointment = await this._healthcareDbContext.Appointment
                 .Include(a => a.Doctor)
                 .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);  // find the specific appointment of the patient

            if (appointment == null) {
                return null;
            };

            return appointment;
        }


        public async Task<Appointment?> GetAppointmentsByIDAndIncludeDoctorAndPatient(int appointmentId)
        {
            Appointment? appointment =  await this._healthcareDbContext.Appointment
                 .Include(a => a.Doctor)
                 .Include(a => a.Patient)
                 .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);  // finding the appointment with the specific id passed

            if (appointment == null)
            {
                return null;
            };

            return appointment;
        }


        public async Task<Appointment?> GetAppointmentById(int appointmentId)
        {
            Appointment? appointment = await this._healthcareDbContext.Appointment
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId); // find the specific appointment of the patient

            if (appointment == null)
            {
                return null;
            }

            return appointment;
        }


        public async Task<Doctor?> GetDoctorById(int doctorId)
        {
            // Get the doctor from the database based on doctor's Id.
            return await this._healthcareDbContext.Doctor
                    .FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }


        public async Task<Availability?> GetDoctorAvailabilityBasedOnAppointmentDateAndDoctorId(DateTime? appointmentDate, int doctorId)
        {
            return await this._healthcareDbContext.Availability
                   .FirstOrDefaultAsync(s => s.SlotDateTime == appointmentDate && s.DoctorId == doctorId);
        }


        public async Task CancelAppointmentAndFreeSlot(Appointment appointment)
        {
            this._healthcareDbContext.Appointment.Remove(appointment);

            await this._healthcareDbContext.SaveChangesAsync();
        }
    }
}
