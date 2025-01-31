using Microsoft.AspNetCore.Mvc;
using PatientsProject.Application.Interfaces;
using PatientsProject.Application.Models;
using System.Net;

namespace PatientsProject.Api.Controllers
{
    /// <summary>
    /// Patients controller
    /// </summary>
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientsController"/> class.
        /// </summary>
        /// <param name="patientService">The service to handle patient operations.</param>
        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Retrieves a list of patients filtered by their birth date.
        /// </summary>
        /// <param name="birthDate">The birth date to filter patients (optional).</param>
        /// <returns>
        /// Returns 200 OK with the list of matching patients.
        /// Returns 400 Bad Request if the birth date format is invalid.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatientDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPatients([FromQuery] string? birthDate)
        {
            IEnumerable<PatientDto> response = await _patientService.GetPatientsAsync(birthDate);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a patient by the unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        /// <returns>
        /// Returns 200 OK with the patient details if found.
        /// Returns 404 Not Found if the patient does not exist.
        /// </returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PatientDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            PatientDto response = await _patientService.GetPatientByIdAsync(id);

            return this.Ok(response);
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="patientDto">The patient details to create.</param>
        /// <returns>
        /// Returns 201 Created with the created patient details.
        /// Returns 400 Bad Request if the model is invalid.
        /// </returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PatientDto>> Create([FromBody] PatientDto patientDto)
        {
            PatientDto patient = await _patientService.AddPatientAsync(patientDto);

            return CreatedAtAction(nameof(GetById), new { id = patient.Name.Id }, patient);
        }

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">The unique identifier of the patient to update.</param>
        /// <param name="patientDto">The updated patient details.</param>
        /// <returns>
        /// Returns 204 No Content on successful update.
        /// Returns 404 Not Found if the patient does not exist.
        /// Returns 400 Bad Request if the model is invalid.
        /// </returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] PatientDto patientDto)
        {
            await _patientService.UpdatePatientAsync(id, patientDto);
            return NoContent();
        }

        /// <summary>
        /// Deletes an existing patient.
        /// </summary>
        /// <param name="id">The unique identifier of the patient to delete.</param>
        /// <returns>
        /// Returns 204 No Content on successful deletion.
        /// Returns 404 Not Found if the patient does not exist.
        /// </returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}
