using System.ComponentModel.DataAnnotations;
using DisprzTraining.Result;
using DisprzTraining.Models;

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
        public Routine Routine{get; set;}
        public bool IsValid(ResultModel check){
            if(this.StartDateTime < DateTime.Now.AddMinutes(-1)) { check.message="Appointment can't set for Past time"; return false;}
            else if(this.StartDateTime >= this.EndDateTime) { check.message = "Start Time should be lesser than End Time"; return false;}
            else if(!Enum.IsDefined(typeof(Routine), this.Routine)) { check.message = "Routine selected is invalid"; return false;}
            else {return true;}
        }
    }
}


