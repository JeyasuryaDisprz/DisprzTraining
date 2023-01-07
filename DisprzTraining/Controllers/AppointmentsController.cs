using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Extensions;
using DisprzTraining.Dto;
using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using DisprzTraining.Result;
=======
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912

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
<<<<<<< HEAD
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
=======
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
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
        }

        [HttpGet("/api/v1/appointments")]
        [ProducesResponseType(typeof(List<Appointment>), 200)]
<<<<<<< HEAD
        [ProducesResponseType(typeof(ErrorModel), 400)]
=======
        [ProducesResponseType(typeof(List<Appointment>), 400)]
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
        public async Task<ActionResult> Get([FromQuery][Required][RegularExpression(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$", ErrorMessage ="Enter date as YYYY-MM-DD format")]string date)
        {
            try{
                var appointmentList = await _appointmentBL.GetAsync(date);
                appointmentList.Select(appointment=>appointment.AsDto());
                return Ok(appointmentList);
            }
            catch{
<<<<<<< HEAD
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
=======
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
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
        {
            // id with startDateTime
            try{
                if (_appointmentBL.Delete(startDateTime))
                {
                    return NoContent();
                }
                else{
<<<<<<< HEAD
                    return NotFound(new ErrorModel(){error = $"Meeting not found at {startDateTime.ToString("hh:mm:tt")}"});
=======
                    return NotFound(new {message = $"Meeting with Id:{id} not found"});
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
                }
            }
            catch{
                return NotFound(new ErrorModel(){error = $"Meeting not Found" });
            }
        }
    }
}
