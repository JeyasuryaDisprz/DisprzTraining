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
                var appointmentList = await _appointmentBL.GetAsync(date);
                appointmentList.Select(appointment=>appointment.AsDto());
                return Ok(appointmentList);
            }
            catch{
                return BadRequest(new ErrorModel(){ error = "Invalid date format. Expected format : YYYY-MM-DD (i.e)2023-01-31"});
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

        // [HttpDelete("/api/v1/appointments/{id}")]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        // [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> Delete(Guid id)
        // {
        //     try{
        //         if (await _appointmentBL.DeleteAsync(id))
        //         {
        //             return NoContent();
        //         }
        //         else{
        //             return NotFound(new ErrorModel(){error = $"Meeting with Id:{id} not found"});
        //         }
        //     }
        //     catch{
        //         return NotFound(new ErrorModel(){error = $"Meeting not Found" });
        //     }
        // }

        [HttpDelete("/api/v1/appointments/{startDateTime}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorModel),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(DateTime startDateTime)
        {
            // id with startDateTime
            try{
                if (_appointmentBL.Delete(startDateTime))
                {
                    return NoContent();
                }
                else{
                    return NotFound(new ErrorModel(){error = $"Meeting not found at {startDateTime.ToString("hh:mm:tt")}"});
                }
            }
            catch{
                return NotFound(new ErrorModel(){error = $"Meeting not Found" });
            }
        }
    }
}
