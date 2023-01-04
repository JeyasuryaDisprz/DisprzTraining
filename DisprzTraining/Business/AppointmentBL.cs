using DisprzTraining.Models;
using DisprzTraining.validation;
using DisprzTraining.Dto;
using DisprzTraining.DataAccess;
using DisprzTraining.Result;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {

        private readonly IAppointmentValidation _appointmentValidation;
        private readonly IAppointmentDAL _appointmentDAL;

        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;
        }

        public async Task<ResultModel> CreateAsync(AppointmentDto appointmentDto)
        {
            try
            {
                return (await _appointmentDAL.CreateAppointmentAsync(appointmentDto));
            }
            catch(Exception e){
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Appointment>> GetAsync(string date)
        {
            return await _appointmentDAL.GetAppointmentAsync(date);
           
        }
        public async Task<Appointment> GetIdAsync(Guid Id)
        {
            return await _appointmentDAL.GetAppointmentByIdAsync(Id);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            return await _appointmentDAL.DeleteAppointmentAsync(Id);
        }
    }
}