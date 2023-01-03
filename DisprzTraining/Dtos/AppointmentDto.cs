using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Dto
{
    public class AppointmentDto
    {
        [Required]
        public DateTime? StartDateTime{ get; set;}
        [Required]
        public DateTime? EndDateTime{ get; set;}
        [Required]
        public string? Title{ get; set;}
        public string? Description{ get; set;}

        
    }
}