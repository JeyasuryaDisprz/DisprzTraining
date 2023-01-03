using DisprzTraining.Models;
using DisprzTraining.Business;
using DisprzTraining.validation;
using DisprzTraining.Dto;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private List<Appointment> Appointments = new(){};
        private readonly IAppointmentValidation _appointmentValidation;

        public AppointmentDAL(IAppointmentValidation appointmentValidation)
        {
            _appointmentValidation = appointmentValidation;
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentDto appointmentDto)
        {
          try
            {
                // bool result = await _appointmentValidation.ExistingAppointments(appointmentDto);
                if (ExistingAppointments(appointmentDto))
                {
                    Appointment appointment = new() {
                        Id = Guid.NewGuid(),
                        Title = appointmentDto.Title,
                        StartDateTime = appointmentDto.StartDateTime,
                        EndDateTime = appointmentDto.EndDateTime,
                        Description = appointmentDto.Description
                    };

                    Appointments.Add(appointment);
                    return true;
                }
                else{
                    return false;
                }
            }
            catch{
                throw new Exception();
            }
        }

        public async Task<List<Appointment>> GetAppointmentAsync(DateTime date)
        {

            // var appointmentListInDate = await _appointmentValidation.FindAppointments(date);
            // appointmentListInDate = appointmentListInDate.OrderBy(x => x.StartDateTime).ToList();

            // if (appointmentListInDate.Count > 0)
            // {
            //     return appointmentListInDate;
            // }

            // return new List<Appointment>();

            return Appointments;

            // return (await Task.FromResult(
            //     (from appointment in Appointments
            //     where appointment.StartDateTime >=date && appointment.StartDateTime < date.AddDays(1)
            //     orderby appointment.StartDateTime ascending
            //     select appointment).ToList()
            // ));

        }

        public async Task<bool> DeleteAppointmentAsync(Guid Id)
        {
            var appoinments = 
                (from appointment in Appointments
                where appointment.Id == Id
                select appointment) as Appointment;

            if (appoinments is not null)
            {
                Appointments.Remove(appoinments);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public bool ExistingAppointments(AppointmentDto appointmentDto){
            foreach (var appointmentDB in Appointments)
            {
                if ((appointmentDto.StartDateTime >= appointmentDB.StartDateTime && appointmentDto.StartDateTime < appointmentDB.EndDateTime) ||
                        (appointmentDto.EndDateTime > appointmentDB.StartDateTime && appointmentDto.EndDateTime <= appointmentDB.EndDateTime))
                {
                    return false;
                }
            }

            return true;
        }
    }
}