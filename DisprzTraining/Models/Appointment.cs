using System;
using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Models
{
    public class Appointment
    {
        [Required]
        public int Id{ get; set;}
        [Required]
        public string? Date{ get; set;}
        [Required]
        public DateTime? StartDateTime{ get; set;}
        [Required]
        public DateTime? EndDateTime{ get; set;}
        [Required]
        public string? Link{ get; set;}

        public Appointment(){
            this.Id = 0;
            this.Date = "";
            this.StartDateTime = DateTime.Now;
            this.EndDateTime = DateTime.Now;
            this.Link = "";
        }
        public Appointment(int Id, string Date, DateTime StartDateTime, DateTime EndDateTime, string Link){
            this.Id = Id;
            this.Date = Date;
            this.StartDateTime = StartDateTime;
            this.EndDateTime = EndDateTime;
            this.Link = Link;
        }
    }
}
