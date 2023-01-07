using DisprzTraining.Models;
using DisprzTraining.validation;
using DisprzTraining.Dto;
using DisprzTraining.DataAccess;
using DisprzTraining.Result;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {

<<<<<<< HEAD
        // private readonly IAppointmentValidation _appointmentValidation;
=======
        private readonly IAppointmentValidation _appointmentValidation;
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
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
<<<<<<< HEAD
        }
        public bool Delete(DateTime startDateTime)
        {
            return  _appointmentDAL.DeleteAppointment(startDateTime);
=======
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
        }
    }
}