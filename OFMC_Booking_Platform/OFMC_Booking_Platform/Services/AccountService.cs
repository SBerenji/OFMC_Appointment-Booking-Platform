using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Services
{
    public class AccountService : IAccountService
    {
        private readonly HealthcareDbContext _healthcareDbContext;

        public AccountService(HealthcareDbContext healthcareDbContext) { 
            this._healthcareDbContext = healthcareDbContext;
        }


        public async Task AddPatientToDB(Patient NewPatient)
        {
            this._healthcareDbContext.Patient.Add(NewPatient);

            await this._healthcareDbContext.SaveChangesAsync();
        }
    }
}
