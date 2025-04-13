using OFMC_Booking_Platform.Entities;

namespace OFMC_Booking_Platform.Services
{
    public interface IAccountService
    {
        Task AddPatientToDB(Patient patient);
    }
}
