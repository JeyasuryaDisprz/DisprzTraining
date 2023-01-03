using System;
using System.Globalization;
using DisprzTraining.Models;
using DisprzTraining.Data;

namespace DisprzTraining.validation
{
    public class AppointmentValidation : IAppointmentValidation
    {

        public async Task<bool> ValideDate(Appointment appointment)
        {
            // if (appointment.StartDateTime < DateTime.Now.Date)
            // {
            //     return await Task.FromResult(false);
            // }

            if ((appointment.StartDateTime >= appointment.EndDateTime))
            {
                throw new Exception("Invalid DateInput, EndTime should be greater than StartTime");
            }

            var dateString = DateOnly.FromDateTime((DateTime)appointment.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");
           
            var AppointmentsInDate = await FindAppointments(stringDate);
            foreach (var appointmentDB in AppointmentsInDate)
            {
                if ((appointment.StartDateTime >= appointmentDB.StartDateTime && appointment.StartDateTime < appointmentDB.EndDateTime) ||
                        (appointment.EndDateTime > appointmentDB.StartDateTime && appointment.EndDateTime <= appointmentDB.EndDateTime))
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