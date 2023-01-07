using System.Globalization;
using DisprzTraining.Models;
using DisprzTraining.Data;
using DisprzTraining.Dto;
<<<<<<< HEAD
using DisprzTraining.Extensions;
=======
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912

namespace DisprzTraining.validation
{
    public class AppointmentValidation : IAppointmentValidation
    {

<<<<<<< HEAD
        // public async Task<Appointment>? ExistingAppointment(AppointmentDto appointmentDto)
        // {          

        //     var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
        //     var stringDate = dateString.ToString("yyyy/MM/dd");
           
        //     var AppointmentsInDate = await FindAppointments(stringDate);
        //     foreach (var appointmentDB in AppointmentsInDate)
        //     {
        //         if (appointmentDB.StartDateTime < appointmentDto.EndDateTime && appointmentDB.EndDateTime > appointmentDto.StartDateTime )
        //         {
        //             return(appointmentDB);
        //         }
        //     }
        //     return (null);
        // }

        // public async Task<bool> ExistingAppointment(AppointmentDto appointmentDto)
        // {          
=======
        public async Task<Appointment> ExistingAppointment(AppointmentDto appointmentDto)
        {          

            var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");
           
            var AppointmentsInDate = await FindAppointments(stringDate);
            foreach (var appointmentDB in AppointmentsInDate)
            {
                if (appointmentDto.StartDateTime < appointmentDB.EndDateTime && appointmentDto.EndDateTime > appointmentDB.StartDateTime )
                {
                    return(appointmentDB);
                }
            }

            return (null);
        }
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912

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

<<<<<<< HEAD
        // public async Task<List<Appointment>> FindAppointments(string? date)
        // {
        //     DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //     return await Task.FromResult(AppointmentData.Appointments.FindAll(x => x.StartDateTime >= dateFormatted && x.StartDateTime < dateFormatted.AddDays(1)));
        // }
=======
        // public async Task<bool> ExistingAppointment(AppointmentDto appointmentDto)
        // {          
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912

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
    }
}