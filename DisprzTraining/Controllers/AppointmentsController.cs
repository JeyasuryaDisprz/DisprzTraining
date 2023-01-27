using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Dto;
using System.ComponentModel.DataAnnotations;
using DisprzTraining.Result;
using System.Net.Mime;

namespace DisprzTraining.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBL _appointmentBL;

        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL = appointmentBL; 
        }

        /// <summary>
        /// Creates an appointment with given Time.
        /// </summary>
        /// <param name="appointmentDto"></param>
        /// <returns>Id or error message</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /appointments
        ///     {
        ///        "startDateTime" : "2023-01-05T22:59:37.289Z",
        ///        "endDateTime" : "2023-01-05T23:59:37.289Z",
        ///        "title": "Appointment Title",
        ///        "description": "Any description/short summary"
        ///        "routine" : 0 (0 - None, 1 - Week, 2 - Month, 2+ - Invalid )
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Created Appointment's Id</response>
        /// <response code="409">Conflict error message with conflicting appointment timings</response>
        /// <response code="400">Returns input validation error messages</response>
        [HttpPost("/api/v1/appointments")]
        [Consumes( MediaTypeNames.Application.Json )]
        [ProducesResponseType(typeof(GuidObject), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public IActionResult Create(AppointmentDto appointmentDto)
        {
            ResultModel check = new(){};
            if (appointmentDto.IsValid(check))
            {
                var result = _appointmentBL.Create(appointmentDto);
                return (result.message == "")? Created("",new GuidObject(){id = result.id}): Conflict(new Error(){error = result.message});
            }
            else
            {
                return BadRequest(new Error(){error = check.message});
            }
        }

        /// <summary>
        /// Get appointments in a given date
        /// </summary>
        /// <param name="date" example="YYYY-MM-DD">Date in format YYYY-MM-DD (i.e)2023-01-31 returns all appointments on that particular date</param>
        /// <returns>A List of appointments</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /appointments?date=2023-01-09
        ///
        /// </remarks>
        /// <response code="200">Returns list of appointments</response>
        /// <response code="400">Returns data validation error messages</response>
        [HttpGet("/api/v1/appointments")]
        [ProducesResponseType(typeof(List<Appointment>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromQuery][Required][RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$", ErrorMessage ="Enter date as YYYY-MM-DD format")]string date)
        {
            try{
                var appointmentList = _appointmentBL.Get(date);
                return Ok(appointmentList);
            }
            catch{
                return BadRequest(new Error(){ error = "Invalid date format. Expected format : YYYY-MM-DD (i.e)2023-01-31"});
            }
        }

        /// <summary>
        /// Delete Appointment with given Id
        /// </summary>
        /// <param name="id">Enter appointment Id(Guid) to delete</param>
        /// <returns>No Content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /appointments/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="404">Appointment not found error message</response>
        [HttpDelete("/api/v1/appointments/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            return (_appointmentBL.Delete(id))? NoContent() : NotFound(new Error(){error = $"Meeting not found at {id}"});
        }

        /// <summary>
        /// Update Appointment with given Id
        /// </summary>
        /// <param name="id">Enter appointment Id(Guid) to update</param>
        /// <returns>No Content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /appointments/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///        "startDateTime" : "2023-01-05T22:59:37.289Z",
        ///        "endDateTime" : "2023-01-05T23:59:37.289Z",
        ///        "title": "Appointment Title",
        ///        "description": "Any description/short summary"
        ///        "routine" : 0
        ///     }
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="409">Conflict error message with conflicting appointment timing</response>
        /// <response code="404">Appointment not found error message</response>
        /// <response code="400">Input data validation error</response>
        [HttpPut("/api/v1/appointments/{id}")]
        [ProducesResponseType(typeof(GuidObject),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status400BadRequest)]
        public IActionResult Update([FromRoute]Guid id,[FromBody]AppointmentDto appointmentDto)
        {
            ResultModel check = new(){};
            if (appointmentDto.IsValid(check))
            {
                var result = _appointmentBL.Update(id,appointmentDto);
                if(result.id != Guid.Empty){
                    return (result.message == "")? NoContent(): Conflict(new Error(){error = result.message});
                }
                else{
                    return NotFound(new Error(){error = result.message});
                }
            }
            else
            {
                return BadRequest(new Error(){error = check.message});
            }
        }

        /// <summary>
        /// Get all routines
        /// </summary>
        /// <returns>A List of routines</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /routines
        ///
        /// </remarks>
        /// <response code="200">Returns list of routines</response>
        [HttpGet("/api/v1/routines")]
        [ProducesResponseType(typeof(List<RoutineDto>), StatusCodes.Status200OK)]
        public IActionResult GetRoutine()
        {
            var appointmentList = _appointmentBL.GetRoutines();
            return Ok(appointmentList);
        }

        /// <summary>
        /// Delete Routine with given Id
        /// </summary>
        /// <param name="id">Enter group Id(Guid) to delete routine</param>
        /// <returns>No Content</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /routines/{3fa85f64-5717-4562-b3fc-2c963f66afa6}
        ///
        /// </remarks>
        /// <response code="204">No Content</response>
        /// <response code="404">Appointment not found error message</response>
        [HttpDelete("/api/v1/routines/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
        public IActionResult DeleteRoutine(Guid id)
        {
            return (_appointmentBL.DeleteRoutine(id))? NoContent() : NotFound(new Error(){error = $"Meeting not found at {id}"});
        }

        /// <summary>
        /// Get Appointment with given Id
        /// </summary>
        /// <param name="id">Enter appointment Id(Guid) to get appointment data</param>
        /// <returns>Appointment data</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /appointments/{3fa85f64-5717-4562-b3fc-2c963f66afa6}
        ///
        /// </remarks>
        /// <response code="200">Returns Appointment Data</response>
        /// <response code="404">Appointment not found error message</response>
        [HttpGet("/api/v1/appointments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error),StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var appointment = _appointmentBL.GetById(id);
            return (appointment.Id != Guid.Empty)? Ok(appointment) : NotFound(new Error(){error = $"Meeting not found at {id}"});
        }
        
    }
}

