using PatientsProject.Application.Models;

namespace PatientsProject.Application.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetPatientsAsync(string? birthDate);
        Task<PatientDto> GetPatientByIdAsync(Guid id);
        Task<PatientDto> AddPatientAsync(PatientDto patientDto);
        Task UpdatePatientAsync(Guid id, PatientDto patientDto);
        Task DeletePatientAsync(Guid id);
    }
}
