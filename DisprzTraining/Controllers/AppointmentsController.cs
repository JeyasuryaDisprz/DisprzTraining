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
        [ProducesResponseType(typeof(ResultModel), 201)]
        [ProducesResponseType(typeof(ResultModel), 409)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult> Create(AppointmentDto appointmentDto)
        {
            ResultModel check = new(){ResultStatusCode= 202};
            if (appointmentDto.IsValid(check))
            {
                var result = await _appointmentBL.CreateAsync(appointmentDto);
                return (result.ResultStatusCode == 201)? Created("",result.Message): Conflict(result.Message);
            //     if (result.ResultStatusCode == 201)
            //     {
            //         return Created("", result.Message);
            //     }
            //     else
            //     {
            //         return Conflict(result.Message);
            //     }
            }
            else
            {
                // return BadRequest(new { message = "Start Time should be lesser than End Time | Appointment can't set for Past time" });
                return BadRequest(check.Message);
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
                return BadRequest("Invalid dateFormat");
            }
        }

        // [HttpGet("/api/v1/appointments/id/{id}")]
        // [ProducesResponseType(typeof(AppointmentDto), 200)]
        // public async Task<ActionResult> GetById(Guid id)
        // {
        //     try{
        //         var appointment = await _appointmentBL.GetIdAsync(id);
        //         var appointmentDto = appointment.AsDto();
        //         return Ok(appointmentDto);
        //     }
        //     catch(Exception e){
        //         return NotFound($"Meeting not Found of Id:{id}");
        //     }
        // }

        [HttpDelete("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
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
