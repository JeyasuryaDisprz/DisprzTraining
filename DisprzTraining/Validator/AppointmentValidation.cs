using System;
using System.Globalization;
using DisprzTraining.Models;
using DisprzTraining.Data;

namespace DisprzTraining.validation
{
    public class AppointmentValidation : IAppointmentValidation
    {

        public async Task<bool> ValideDate(Appointment appoinment)
        {
            // if (appoinment.StartDateTime < DateTime.Now.Date)
            // {
            //     return await Task.FromResult(false);
            // }

            var dateString = DateOnly.FromDateTime((DateTime)appoinment.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");
           
            var AppointmentsInDate = await FindAppointments(stringDate);
            foreach (var appoinmentDB in AppointmentsInDate)
            {
                if ((appoinment.StartDateTime >= appoinmentDB.StartDateTime && appoinment.StartDateTime < appoinmentDB.EndDateTime) ||
                        (appoinment.EndDateTime > appoinmentDB.StartDateTime && appoinment.EndDateTime <= appoinmentDB.EndDateTime))
                {
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<List<Appointment>> FindAppointments(string? date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return await Task.FromResult(AppointmentData.Appointments.FindAll(x => x.StartDateTime >= dateFormatted && x.StartDateTime < dateFormatted.AddDays(1)));
        }

        public async Task<Appointment> FindAppointment(Guid Id)
        {
            return await Task.FromResult(AppointmentData.Appointments.Find(x => x.Id == Id));
        }

    }
}