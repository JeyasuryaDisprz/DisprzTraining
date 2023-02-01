using DisprzTraining.Dto;
using DisprzTraining.Models;

namespace Extension
{
    public static class Extension
    {
        public static Appointment ToAppointment(this AppointmentDto appointmentDto, Guid Id)
        {
            if(Id == Guid.Empty){
                Id = Guid.NewGuid();
            }
            
            return(
                new Appointment(){
                    Id = Id,
                    StartDateTime = appointmentDto.StartDateTime,
                    EndDateTime = appointmentDto.EndDateTime,
                    Title = appointmentDto.Title,
                    Description = appointmentDto.Description,
                    Routine = appointmentDto.Routine,
                }
            );
        }
    }
}