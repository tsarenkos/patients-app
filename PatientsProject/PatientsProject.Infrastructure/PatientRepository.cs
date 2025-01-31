using Microsoft.EntityFrameworkCore;
using PatientsProject.Application.Interfaces;
using PatientsProject.Domain.Entities;

namespace PatientsProject.Infrastructure
{
    public class PatientRepository: IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllAsync() => await _context.Patients.ToListAsync();

        public async Task<Patient?> GetByIdAsync(Guid id) => await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Patient>> SearchByBirthDateAsync(string prefix, DateTime date)
        {
            IQueryable<Patient> query = _context.Patients;

            DateTime dateStart, dateEnd;
           
            if (date.TimeOfDay == TimeSpan.Zero)  // User provided YYYY-MM-DD
            {
                dateStart = date;
                dateEnd = date.AddDays(1).AddTicks(-1);
            }
            else if (date.Second == 0)  // User provided YYYY-MM-DDTHH:MM
            {
                dateStart = date;
                dateEnd = date.AddMinutes(1).AddTicks(-1);
            }
            else  // User provided full YYYY-MM-DDTHH:MM:SS
            {
                dateStart = date;
                dateEnd = date.AddSeconds(1).AddTicks(-1);
            }

            query = prefix switch
            {
                // Exact match
                "eq" => query.Where(p => p.BirthDate >= dateStart && p.BirthDate <= dateEnd),

                // Not equal
                "ne" => query.Where(p => p.BirthDate < dateStart || p.BirthDate > dateEnd),

                // Less than
                "lt" => query.Where(p => p.BirthDate < dateStart),

                // Greater than
                "gt" => query.Where(p => p.BirthDate > dateEnd),

                // Less than or equal
                "le" => query.Where(p => p.BirthDate <= dateEnd),

                // Greater than or equal
                "ge" => query.Where(p => p.BirthDate >= dateStart),

                // "sa" - Same as: Birthdate is on or after the given date
                "sa" => query.Where(p => p.BirthDate >= dateStart),

                // "eb" - Ends before: Birthdate is before the given date
                "eb" => query.Where(p => p.BirthDate < dateStart),

                // "ap" - Approximate: "near" as +/- 7 days around the given date               
                "ap" => query.Where(p => p.BirthDate >= dateStart.AddDays(-7) && p.BirthDate <= dateEnd.AddDays(7)),

                _ => throw new ArgumentException("Invalid prefix.")
            };

            return await query.ToListAsync();
        }

        public async Task<Patient> AddAsync(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }
    }
}
