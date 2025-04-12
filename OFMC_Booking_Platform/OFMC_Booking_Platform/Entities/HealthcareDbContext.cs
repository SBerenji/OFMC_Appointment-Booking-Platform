using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OFMC_Booking_Platform.Entities
{
    public class HealthcareDbContext : IdentityDbContext<User> //Database context entity that inherits from DbContext
    {
        public HealthcareDbContext(DbContextOptions<HealthcareDbContext> options) : base(options) { }

        // Party table in database
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Availability> Availability { get; set; }


        // Seeds data into database tables 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seeds data into the Party table 
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor() { DoctorId = 1, DoctorName = "Dr. Emily Carter", DoctorSpecialty = "Family Physician", DoctorExt = 106 },
                new Doctor() { DoctorId = 2, DoctorName = "Dr. Shawn Kieze", DoctorSpecialty = "Pediatrics", DoctorExt = 103 },
                new Doctor() { DoctorId = 3, DoctorName = "Dr. Sophia Lee", DoctorSpecialty = "Women's Health & OB-GYN", DoctorExt = 102 },
                new Doctor() { DoctorId = 4, DoctorName = "Dr. James Thompson", DoctorSpecialty = "Internal Medicine", DoctorExt = 104 },
                new Doctor() { DoctorId = 5, DoctorName = "Dr. Olivia Martinez", DoctorSpecialty = "Dermatology", DoctorExt = 105 },
                new Doctor() { DoctorId = 6, DoctorName = "Dr. Ryan Patel", DoctorSpecialty = "Family Physician", DoctorExt = 101 }
                );

            // Seeds data into the Party table 
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment() { AppointmentId = 1, DoctorId=1, PatientId=1, PatientName="Sara Hanks", AppointmentDate= new DateTime(2022, 12, 31, 12, 0, 0), ContactMethod=ContactMethod.Email, AppointmentEmail= "sierraerb25@gmail.com", AppointmentPhone = "+15483335882", Notes="Headache" }
               );

            // Seeds data into the Party table 
            modelBuilder.Entity<Patient>().HasData(
                new Patient() { PatientId = 1, FirstName = "Sierra", LastName="Erb", DOB= new DateTime(2022, 12, 31, 12, 0, 0), Password="password", PatientEmail="sierraerb25@gmail.com" }
               );

            // Seeds data into the Party table 
            modelBuilder.Entity<Availability>().HasData(
                new Availability() { SlotId = 1, DoctorId = 1, SlotDateTime = new DateTime(2025, 10, 28, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 2, DoctorId = 1, SlotDateTime = new DateTime(2025, 11, 15, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 3, DoctorId = 1, SlotDateTime = new DateTime(2025, 12, 12, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 4, DoctorId = 2, SlotDateTime = new DateTime(2025, 10, 05, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 5, DoctorId = 2, SlotDateTime = new DateTime(2025, 11, 11, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 6, DoctorId = 2, SlotDateTime = new DateTime(2025, 12, 16, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 7, DoctorId = 3, SlotDateTime = new DateTime(2025, 10, 20, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 8, DoctorId = 3, SlotDateTime = new DateTime(2025, 11, 28, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 9, DoctorId = 3, SlotDateTime = new DateTime(2025, 12, 22, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 10, DoctorId = 4, SlotDateTime = new DateTime(2025, 10, 20, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 11, DoctorId = 4, SlotDateTime = new DateTime(2025, 11, 15, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 12, DoctorId = 4, SlotDateTime = new DateTime(2025, 12, 19, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 13, DoctorId = 5, SlotDateTime = new DateTime(2025, 10, 02, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 14, DoctorId = 5, SlotDateTime = new DateTime(2025, 11, 15, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 15, DoctorId = 5, SlotDateTime = new DateTime(2025, 12, 14, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 16, DoctorId = 6, SlotDateTime = new DateTime(2025, 10, 17, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 17, DoctorId = 6, SlotDateTime = new DateTime(2025, 11, 25, 12, 0, 0), IsBooked = false },
                new Availability() { SlotId = 18, DoctorId = 6, SlotDateTime = new DateTime(2025, 12, 20, 12, 0, 0), IsBooked = false }
                );

        }
    }
}
