using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        [ProducesResponseType(typeof(Appointment), 200)]
        public async Task<OkObjectResult> Get(string date)
        {
            var appointmentList = await _appointmentBL.GetAsync(date);
            return Ok(appointmentList);
        }

        [HttpGet("/api/v1/appointments/id/{id}")]
        [ProducesResponseType(typeof(Appointment), 200)]
        public async Task<OkObjectResult> Get(Guid id)
        {
            var appoinment = await _appointmentBL.GetIdAsync(id);
            return Ok(appoinment);
        }

        [HttpPost("/api/v1/appointments")]
        [ProducesResponseType(typeof(Appointment), 201)]
        [ProducesResponseType(typeof(Appointment), 409)]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if(await _appointmentBL.CreateAsync(appointment)){
                return Created("Appointment is successfully created",appointment);
            }
            return Conflict(new { message = $"There is an appointment already found."});
        }

        [HttpDelete("/api/v1/appointment/{Id}")]
        [ProducesResponseType(typeof(Appointment), 200)]
        [ProducesResponseType(typeof(Appointment), 404)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            if (await _appointmentBL.DeleteAsync(Id))
            {
                return Ok();
            }
            return NotFound(new { message = $"Error on delete" });
        }
    }
}
