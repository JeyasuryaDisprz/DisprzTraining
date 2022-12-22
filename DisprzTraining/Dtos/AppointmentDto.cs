using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Dto
{
    public class AppointmentDto
    {
        public Guid Id{ get; init;}
        public DateTime? StartDateTime{ get; set;}
        public DateTime? EndDateTime{ get; set;}
        public string? Title{ get; set;}
    }
}