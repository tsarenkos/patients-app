using Newtonsoft.Json;

namespace PatientsProject.Application.Models
{
    public class PatientName
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("use")]
        public string? Use { get; set; }

        [JsonProperty("family")]
        public string Family { get; set; }

        [JsonProperty("given")]
        public string[]? Given { get; set; }
    }
}
