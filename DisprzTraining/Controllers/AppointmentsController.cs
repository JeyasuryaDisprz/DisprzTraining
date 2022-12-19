using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Business;
using DisprzTraining.Data;
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

        [HttpGet("/api/v1/{date}")]
        [ProducesResponseType(typeof(Appointment), 200)]
        public async Task<OkObjectResult> Get(string date)
        {
            var appointmentList = await _appointmentBL.GetAsync(date);
            return Ok(appointmentList);
        }

        [HttpPost("/api/v1")]
        [ProducesResponseType(typeof(Appointment), 201)]
        [ProducesResponseType(typeof(Appointment), 409)]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if(await _appointmentBL.CreateAsync(appointment)){
                return Created("Appointment is successfully created",appointment);
            }
            return Conflict(new { message = $"There is an appointment already found."});
        }

        [HttpDelete("/api/v1/{Id}")]
        [ProducesResponseType(typeof(Appointment), 200)]
        [ProducesResponseType(typeof(Appointment), 404)]
        public async Task<IActionResult> Delete(int Id)
        {
            if (await _appointmentBL.DeleteAsync(Id))
            {
                return Ok();
            }
            return NotFound(new { message = $"Error on delete" });
        }
    }
}
