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

        [HttpGet("/api/v1/appointments/{date}")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        public async Task<OkObjectResult> Get(string date)
        {
            var appointmentList = await _appointmentBL.GetAsync(date);
            appointmentList.Select(appointment=>appointment.AsDto());
            return Ok(appointmentList);
        }

        [HttpGet("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        public async Task<OkObjectResult> GetById(Guid id)
        {
            var appointment = await _appointmentBL.GetIdAsync(id);
            var appointmentDto = appointment.AsDto();
            return Ok(appointmentDto);
        }

        [HttpPost("/api/v1/appointments")]
        [ProducesResponseType(typeof(AppointmentDto), 201)]
        [ProducesResponseType(typeof(AppointmentDto), 409)]
        public async Task<ActionResult> Create(AppointmentDto appointmentDto)
        {
            Appointment appointment = new(){
                Id = appointmentDto.Id,
                StartDateTime = appointmentDto.StartDateTime,
                EndDateTime = appointmentDto.EndDateTime,
                Title = appointmentDto.Title
            };
                 bool conflict =   await _appointmentBL.CreateAsync(appointment);
            if(conflict){
                return CreatedAtAction(nameof(GetById), new {Id = appointment.Id}, appointment.AsDto());
            }
            else{
            return Conflict("Meet already found");
            }
        }

        [HttpDelete("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(typeof(AppointmentDto), 200)]
        [ProducesResponseType(typeof(AppointmentDto), 404)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _appointmentBL.DeleteAsync(id))
            {
                return Ok();
            }
            return NotFound(new { message = $"Error on delete" });
        }
    }
}
