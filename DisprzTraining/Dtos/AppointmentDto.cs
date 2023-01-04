using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Dto
{
    public class AppointmentDto
    {
        [Required(ErrorMessage = "Start DateTime is required")]
        public DateTime? StartDateTime{ get; set;}
        [Required(ErrorMessage = "End DateTime is required")]
        public DateTime? EndDateTime{ get; set;}
        [Required(ErrorMessage = "Title is required")]
        public string? Title{ get; set;}
        public string? Description{ get; set;}

        public bool IsValid(){
            return (this.StartDateTime < this.EndDateTime)?true:false;
        }
        
    }
}