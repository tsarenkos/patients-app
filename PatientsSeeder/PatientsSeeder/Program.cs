using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

const string API_URL = "http://patient_api:5000/api/patients";

HttpClient _httpClient = new();
Console.WriteLine("Generating 100 patients...");

var tasks = new List<Task>();
for (int i = 0; i < 100; i++)
{
    var patient = GenerateRandomPatient();
    tasks.Add(SendPatientToApi(patient));
}

await Task.WhenAll(tasks);

Console.WriteLine("All patients have been created!");

static Patient GenerateRandomPatient()
{
    var random = new Random();
    var firstNames = new[] { "Ivan", "John", "Aleh", "Maria", "Maksim", "Andrey", "Elena", "James", "Peter", "Anna" };
    var lastNames = new[] { "Ivanov", "Smith", "Korolev", "Brown", "Haker", "Andreev", "Petrova", "Born", "Davis", "Lopez" };    

    return new Patient
    {
        Name = new PatientName
        {
            Id = Guid.NewGuid(),
            Use = "official",
            Family = lastNames[random.Next(lastNames.Length)],
            Given = new[] { firstNames[random.Next(firstNames.Length)], "unknown" }
        },
        Gender = random.Next(2) == 0 ? "male" : "female",
        BirthDate = DateTime.UtcNow.AddYears(-random.Next(1, 100)),
        Active = random.Next(2) == 0
    };
}

async Task SendPatientToApi(Patient patient)
{
    try
    {
        var json = System.Text.Json.JsonSerializer.Serialize(patient, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(API_URL, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Patient {patient.Name.Family} added successfully.");
        }
        else
        {
            Console.WriteLine($"Error adding patient {patient.Name.Family}: {await response.Content.ReadAsStringAsync()}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occured: {ex.Message}");
    }
}

// Model classes
public class Patient
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