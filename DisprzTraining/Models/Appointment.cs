using System.ComponentModel.DataAnnotations;
using DisprzTraining.Dto;

namespace DisprzTraining.Models
{
    public class Appointment : AppointmentDto
    {
        [Required]
        public Guid Id{ get; set;}
        public Guid GroupId{get; set;}
    }
}
