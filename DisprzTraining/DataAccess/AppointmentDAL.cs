using DisprzTraining.Models;
using DisprzTraining.validation;
using DisprzTraining.Data;
using DisprzTraining.Dto;
using System.Globalization;
using DisprzTraining.Result;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private readonly IAppointmentValidation _appointmentValidation;

        public AppointmentDAL(IAppointmentValidation appointmentValidation)
        {
            _appointmentValidation = appointmentValidation;
        }

        public async Task<ResultModel> CreateAppointmentAsync(AppointmentDto appointmentDto)
        {
            Appointment result = await _appointmentValidation.ExistingAppointment(appointmentDto);
            if (result is null)
            {
                var appointment = new Appointment()
                {
                    Id = Guid.NewGuid(),
                    StartDateTime = appointmentDto.StartDateTime,
                    EndDateTime = appointmentDto.EndDateTime,
                    Title = appointmentDto.Title,
                    Description = appointmentDto.Description
                };

                AppointmentData.Appointments.Add(appointment);
                return new ResultModel(){ResultStatusCode = 201, appointment = appointment};
            }
            else
            {
                return new ResultModel(){ResultStatusCode = 409, appointment = result};
            }
        }

        public async Task<List<Appointment>> GetAppointmentAsync(string date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return 
                (from appointment in AppointmentData.Appointments
                where appointment.StartDateTime >= dateFormatted && appointment.EndDateTime < dateFormatted.AddDays(1)
                orderby appointment.StartDateTime ascending
                select appointment).ToList();

        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid Id){
            return
                (from appointment in AppointmentData.Appointments
                where appointment.Id == Id
                select appointment).FirstOrDefault() as Appointment;
        }
        public async Task<bool> DeleteAppointmentAsync(Guid Id)
        {
            var appoinment = AppointmentData.Appointments.Where(appoinment => appoinment.Id == Id).SingleOrDefault();

            if (appoinment is not null)
            {
                AppointmentData.Appointments.Remove((Appointment)appoinment);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        // public bool ExistingAppointments(AppointmentDto appointmentDto){
        //     foreach (var appointmentDB in Appointments)
        //     {
        //         if ((appointmentDto.StartDateTime >= appointmentDB.StartDateTime && appointmentDto.StartDateTime < appointmentDB.EndDateTime) ||
        //                 (appointmentDto.EndDateTime > appointmentDB.StartDateTime && appointmentDto.EndDateTime <= appointmentDB.EndDateTime))
        //         {
        //             return false;
        //         }
        //     }

        //     return true;
        // }
    }
}