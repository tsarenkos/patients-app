using PatientsProject.Application.Exceptions;
using PatientsProject.Application.Interfaces;
using PatientsProject.Application.Models;
using PatientsProject.Domain.Entities;
using PatientsProject.Domain.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PatientsProject.Application.Services
{
    public class PatientService: IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private const string dateFilterPattern = @"^(eq|ne|lt|gt|le|ge|sa|eb|ap)(\d{4}-\d{2}-\d{2}(?:T\d{2}:\d{2})?)$";

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientDto>> GetPatientsAsync(string? birthDate)
        {
            if (string.IsNullOrEmpty(birthDate))
            {
                return (await _patientRepository.GetAllAsync())
                    .Select(PatientExtensions.CreateDtoModelFromEntity)
                    .ToList();
            }
            
            var match = Regex.Match(birthDate, dateFilterPattern);

            if (!match.Success)
            {
                throw new ApplicationValidationException("birthDate",
                    "Invalid date filter format. Use eq, ne, lt, gt, le, ge, sa, eb, ap with YYYY-MM-DD or YYYY-MM-DDTHH:MM.");
            }

            string prefix = match.Groups[1].Value;
            string dateString = match.Groups[2].Value;

            if (!DateTime.TryParseExact(dateString, 
                    dateString.Contains('T') ? "yyyy-MM-ddTHH:mm" : "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out DateTime dateValue))
            {
                throw new ApplicationValidationException("birthDate", "Invalid date format.");
            }

            return (await _patientRepository.SearchByBirthDateAsync(prefix, dateValue))
                .Select(PatientExtensions.CreateDtoModelFromEntity)
                .ToList();
        }

        public async Task<PatientDto> GetPatientByIdAsync(Guid id)
        {
            Patient? patient = await _patientRepository.GetByIdAsync(id) 
                ?? throw new NotFoundException($"Patient with id = {id} is not found");

            return PatientExtensions.CreateDtoModelFromEntity(patient);
        }

        public async Task<PatientDto> AddPatientAsync(PatientDto patientDto)
        {
            this.ValidatePatientDtoModel(patientDto);

            Patient patient = PatientExtensions.CreatePatientFromDtoModel(patientDto);
            await _patientRepository.AddAsync(patient);

            return PatientExtensions.CreateDtoModelFromEntity(patient);            
        }

        public async Task UpdatePatientAsync(Guid id, PatientDto patientDto)
        {
            this.ValidatePatientDtoModel(patientDto);

            Patient? existingPatient = await _patientRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Patient with id = {id} is not found");

            PatientExtensions.UpdatePatientFromDtoModel(patientDto, existingPatient);            

            await _patientRepository.UpdateAsync(existingPatient);
        }

        public async Task DeletePatientAsync(Guid id)
        {
            Patient? existingPatient = await _patientRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Patient with id = {id} is not found");

            await _patientRepository.DeleteAsync(existingPatient);
        }

        private bool ValidatePatientDtoModel(PatientDto patientDto)
        {
            if (string.IsNullOrEmpty(patientDto.Name?.Family))
            {
                throw new ApplicationValidationException("family", "This field can't be empty.");
            }

            if (!Enum.TryParse(typeof(Gender), patientDto.Gender, true, out var result))
            {
                throw new ApplicationValidationException("gender", "Invalid gender format. Allowed values: male, female, other, unknown.");
            }

            return true;
        }
    }
}
