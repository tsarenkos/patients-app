using Newtonsoft.Json;
using PatientsProject.Domain.Enums;

namespace PatientsProject.Application.Models
{
    public class PatientDto
    {
        [JsonProperty("name")]
        public PatientName Name { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
