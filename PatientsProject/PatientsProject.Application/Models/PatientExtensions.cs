using PatientsProject.Domain.Entities;
using PatientsProject.Domain.Enums;

namespace PatientsProject.Application.Models
{
    public static class PatientExtensions
    {
        public static Patient CreatePatientFromDtoModel(this PatientDto dtoModel)
        {
            (string? firstName, string? lastName) = GetNames(dtoModel.Name.Given);
            Gender gender = ParseGender(dtoModel.Gender);

            return new Patient()
            {
                Id = dtoModel.Name.Id,
                LastName = dtoModel.Name.Family,
                FirstName = firstName,
                MiddleName = lastName,
                Use = dtoModel.Name.Use,
                BirthDate = dtoModel.BirthDate,
                Active = dtoModel.Active,
                Gender = gender
            };
        }

        public static void UpdatePatientFromDtoModel(this PatientDto dtoModel, Patient patient)
        {
            ArgumentNullException.ThrowIfNull(patient);

            (string? firstName, string? lastName) = GetNames(dtoModel.Name.Given);
            Gender gender = ParseGender(dtoModel.Gender);

            patient.LastName = dtoModel.Name.Family;
            patient.FirstName = firstName;
            patient.MiddleName = lastName;
            patient.Use = dtoModel.Name.Use;
            patient.Gender = gender;
            patient.Active = dtoModel.Active;
            patient.BirthDate = dtoModel.BirthDate;
        }

        public static PatientDto CreateDtoModelFromEntity(this Patient patient)
        {           
            string gender = Enum.GetName(patient.Gender)?.ToLower() ?? "unknown";

            PatientDto dtoModel = new()
            {
                Name = new PatientName
                {
                    Id = patient.Id,
                    Given = new string[] { patient.FirstName, patient.MiddleName },
                    Family = patient.LastName,
                    Use = patient.Use,
                },
                BirthDate = patient.BirthDate,
                Active = patient.Active,
                Gender = gender,
            };

            return dtoModel;
        }

        private static (string? firstName, string? lastName) GetNames(string[]? given)
        {
            if (given is null || given.Length == 0)
            {
                return (null, null);
            }

            if (given.Length == 1)
            {
                return (given[0], null);
            }

            return (given[0], given[1]);
        }

        private static Gender ParseGender(string? genderString)
        {
            if (string.IsNullOrEmpty(genderString) || !Enum.TryParse(genderString, true, out Gender gender))
            {
                return Gender.Unknown; // Return default 'Unknown' if parsing fails or string is empty
            }

            return gender;
        }
    }
}
