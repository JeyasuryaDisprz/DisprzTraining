using System.Globalization;
using DisprzTraining.Models;
using DisprzTraining.Data;
using DisprzTraining.Dto;

namespace DisprzTraining.validation
{
    public class AppointmentValidation : IAppointmentValidation
    {

        public async Task<Appointment> ExistingAppointment(AppointmentDto appointmentDto)
        {          

            var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");
           
            var AppointmentsInDate = await FindAppointments(stringDate);
            foreach (var appointmentDB in AppointmentsInDate)
            {
                if (appointmentDB.StartDateTime < appointmentDto.EndDateTime && appointmentDB.EndDateTime > appointmentDto.StartDateTime )
                {
                    return(appointmentDB);
                }
            }

            return (null);
        }

        // public async Task<bool> ExistingAppointment(AppointmentDto appointmentDto)
        // {          

        //     var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
        //     var stringDate = dateString.ToString("yyyy/MM/dd");
           
        //     var AppointmentsInDate = await FindAppointments(stringDate);
        //     foreach (var appointmentDB in AppointmentsInDate)
        //     {
        //         if ((appointmentDto.StartDateTime >= appointmentDB.StartDateTime && appointmentDto.StartDateTime < appointmentDB.EndDateTime) ||
        //                 (appointmentDto.EndDateTime > appointmentDB.StartDateTime && appointmentDto.EndDateTime <= appointmentDB.EndDateTime))
        //         {
        //             return await Task.FromResult(false);
        //         }
        //     }

        //     return await Task.FromResult(true);
        // }

        public async Task<List<Appointment>> FindAppointments(string? date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return await Task.FromResult(AppointmentData.Appointments.FindAll(x => x.StartDateTime >= dateFormatted && x.StartDateTime < dateFormatted.AddDays(1)));
        }

    }
}