using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DisprzTraining.Extensions;
using DisprzTraining.Dto;

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
        [ProducesResponseType(typeof(AppointmentDto), 400)]
        [ProducesResponseType(typeof(AppointmentDto), 409)]
        public async Task<ActionResult> Create(AppointmentDto appointmentDto)
        {
            Appointment appointment = new(){
                Id = Guid.NewGuid(),
                StartDateTime = appointmentDto.StartDateTime,
                EndDateTime = appointmentDto.EndDateTime,
                Title = appointmentDto.Title,
                Description = appointmentDto.Description
            };
            
            try{
                bool noConflict =   await _appointmentBL.CreateAsync(appointment);
                if(noConflict){
                    return CreatedAtAction(nameof(GetById), new {Id = appointment.Id}, appointmentDto);
                }
                else{
                    return Conflict("Meet already found");
                }
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [HttpGet("/api/v1/appointments/")]
        [ProducesResponseType(typeof(List<Appointment>), 200)]
        [ProducesResponseType(typeof(List<Appointment>), 400)]
        public async Task<ActionResult> Get([FromQuery]string date)
        {
            try{
                var appointmentList = await _appointmentBL.GetAsync(date);
                appointmentList.Select(appointment=>appointment.AsDto());
                return Ok(appointmentList);
            }
            catch{
                return BadRequest("Invalid data");
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
            catch{
                return NotFound("Meeting not Found");
            }
        }

        [HttpDelete("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(typeof(AppointmentDto), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try{
                if (await _appointmentBL.DeleteAsync(id))
                {
                    return Ok();
                }
            }
            catch{
                return NotFound(new { message = $"Meeting not Found" });
            }
            return Ok();
        }
    }
}
