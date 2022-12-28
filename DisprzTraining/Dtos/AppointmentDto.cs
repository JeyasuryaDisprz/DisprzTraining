using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Dto
{
    public class AppointmentDto
    {
        public DateTime? StartDateTime{ get; set;}
        public DateTime? EndDateTime{ get; set;}
        public string? Title{ get; set;}
    }
}