using PatientsProject.Domain.Enums;

namespace PatientsProject.Domain.Entities
{
    public class Patient
    {       
        public Guid Id { get; set; } = Guid.NewGuid();
   
        public string LastName { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? Use { get; set; }
    
        public Gender Gender { get; set; }
      
        public DateTime BirthDate { get; set; }

        public bool Active { get; set; }
    }
}
