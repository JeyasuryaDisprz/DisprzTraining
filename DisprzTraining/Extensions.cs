using DisprzTraining.Dto;
using DisprzTraining.Models;

namespace DisprzTraining.Extensions
{
    public static class Extensions
    {
        public static AppointmentDto AsDto(this Appointment appointment)
        {
            return new AppointmentDto{
                StartDateTime = appointment.StartDateTime,
                EndDateTime = appointment.EndDateTime,
                Title = appointment.Title
            };
        }
    }
}