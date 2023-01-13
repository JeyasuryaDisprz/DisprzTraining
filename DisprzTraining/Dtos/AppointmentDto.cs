using System.ComponentModel.DataAnnotations;
using DisprzTraining.Result;

namespace DisprzTraining.Dto
{
    public class AppointmentDto
    {
        [Required(ErrorMessage = "Start DateTime is required")]
        public DateTime StartDateTime{ get; set;}
        [Required(ErrorMessage = "End DateTime is required")]
        public DateTime EndDateTime{ get; set;}
        [Required(ErrorMessage = "Title is required")]
        public string Title{ get; set;}
        public string? Description{ get; set;}

        public bool IsValid(ResultModel check){
            // return (this.StartDateTime >= DateTime.Now && this.StartDateTime < this.EndDateTime)? true: false;

            if(this.StartDateTime < DateTime.Now.AddMinutes(-1)) { check.ErrorMessage=$"Appointment can't set for Past time"; return false;}
            else if(this.StartDateTime >= this.EndDateTime) { check.ErrorMessage = $"Start Time should be lesser than End Time"; return false;}
            else {return true;}
        }
    }
}