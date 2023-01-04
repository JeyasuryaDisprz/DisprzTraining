using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Extensions;
using DisprzTraining.Dto;
using System.ComponentModel.DataAnnotations;

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
        [ProducesResponseType(typeof(AppointmentDto), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 409)]
        public async Task<ActionResult> Create(AppointmentDto appointmentDto)
        {
            if(appointmentDto.IsValid()){
                var result =   await _appointmentBL.CreateAsync(appointmentDto);
                if(result.ResultStatusCode == 201){
                    return Created("", new {message = "Appointment added", id = result.appointment.Id});
                }
                else{
                    return Conflict($"Meet already found at {result.appointment.StartDateTime} - {result.appointment.EndDateTime}");
                }
                // if(result.StartDateTime == appointmentDto.StartDateTime){
                //     return Created("", new {message = "Appointment added", id = result.Id});
                // }
                // else{
                //     return Conflict($"Meet already found at {result.StartDateTime} - {result.EndDateTime}");
                // }
            }
            else{
                return BadRequest(new {message = "Start DateTime should be greater than End DateTime"});
            }     
        }

        [HttpGet("/api/v1/appointments/")]
        [ProducesResponseType(typeof(List<Appointment>), 200)]
        [ProducesResponseType(typeof(List<Appointment>), 400)]
        public async Task<ActionResult> Get([FromQuery][Required][RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$", ErrorMessage ="Enter date as YYYY-MM-DD format")]string date)
        {
            try{
                var appointmentList = await _appointmentBL.GetAsync(date);
                appointmentList.Select(appointment=>appointment.AsDto());
                return Ok(appointmentList);
            }
            catch{
                return BadRequest("Invalid date");
            }
        }

        [HttpGet("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        public async Task<ActionResult> GetById(Guid id)
        {
            try{
                var appointment = await _appointmentBL.GetIdAsync(id);
                var appointmentDto = appointment.AsDto();
                return Ok(appointmentDto);
            }
            catch(Exception e){
                return NotFound($"Meeting not Found of Id:{id}");
            }
        }

        [HttpDelete("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try{
                if (await _appointmentBL.DeleteAsync(id))
                {
                    return NoContent();
                }
                else{
                    return NotFound(new {message = $"Meeting with Id:{id} not found"});
                }
            }
            catch{
                return NotFound(new { message = $"Meeting not Found" });
            }
        }
    }
}
