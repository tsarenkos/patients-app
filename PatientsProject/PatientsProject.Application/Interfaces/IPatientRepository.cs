using PatientsProject.Application.Models;
using PatientsProject.Domain.Entities;

namespace PatientsProject.Application.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(Guid id);
        Task<IEnumerable<Patient>> SearchByBirthDateAsync(string prefix, DateTime date);
        Task<Patient> AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
    }
}
