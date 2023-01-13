using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Extensions;
using DisprzTraining.Dto;
using System.ComponentModel.DataAnnotations;
using DisprzTraining.Result;

namespace DisprzTraining.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBL _appointmentBL;

        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL = appointmentBL; 
        }

        [HttpPost("/api/v1/appointments")]
        [ProducesResponseType(typeof(CreatedModel), 201)]
        [ProducesResponseType(typeof(ErrorModel), 409)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<ActionResult> Create(AppointmentDto appointmentDto)
        {
            ResultModel check = new(){};
            if (appointmentDto.IsValid(check))
            {
                var result = await _appointmentBL.CreateAsync(appointmentDto);
                return (result.ErrorMessage == "")? Created("",new CreatedModel(){Id = result.appointmentId}): Conflict(new ErrorModel(){error = result.ErrorMessage});
            }
            else
            {
                return BadRequest(new ErrorModel(){error = check.ErrorMessage});
            }
        }

        [HttpGet("/api/v1/appointments")]
        [ProducesResponseType(typeof(List<Appointment>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 400)]
        public async Task<ActionResult> Get([FromQuery][Required][RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$", ErrorMessage ="Enter date as YYYY-MM-DD format")]string date)
        {
            try{
                var appointmentList = _appointmentBL.GetAsync(date);
                appointmentList.Select(appointment=>appointment.AsDto());
                return Ok(appointmentList);
            }
            catch{
                return BadRequest(new ErrorModel(){ error = "Invalid date format. Expected format : YYYY-MM-DD (i.e)2023-01-31"});
            }
        }

        [HttpDelete("/api/v1/appointments/{StartDateTime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(DateTime StartDateTime)
        {
            // id with startDateTime
            try{
                if (_appointmentBL.Delete(StartDateTime))
                {
                    return NoContent();
                }
                else{
                    return NotFound(new ErrorModel(){error = $"Meeting not found at {StartDateTime.ToString("hh:mm:tt")}"});
                }
            }
            catch{
                return NotFound(new ErrorModel(){error = $"Meeting not Found" });
            }
        }

        [HttpPut("/api/v1/appointments/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute]Guid id,[FromBody]AppointmentDto appointmentDto)
        {
            ResultModel check = new(){};
            if (appointmentDto.IsValid(check))
            {
                var result = await _appointmentBL.Update(id,appointmentDto);
                if(result.appointmentId != Guid.Empty){
                    return (result.ErrorMessage == "")? Ok(new UpdateModel(){Id = result.appointmentId}): Conflict(new ErrorModel(){error = result.ErrorMessage});
                }
                else{
                    return NotFound(new ErrorModel(){error = result.ErrorMessage});
                }
            }
            else
            {
                return BadRequest(new ErrorModel(){error = check.ErrorMessage});
            }
        }
    }
}
