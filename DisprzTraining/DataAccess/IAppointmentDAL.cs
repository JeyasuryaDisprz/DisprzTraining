using DisprzTraining.Dto;
using DisprzTraining.Models;
using DisprzTraining.Result;

namespace DisprzTraining.DataAccess{
    public interface IAppointmentDAL{
        public Task<ResultModel> CreateAppointmentAsync(AppointmentDto appointmentDto);
        public Task<List<Appointment>> GetAppointmentAsync(string date);
        public Task<Appointment> GetAppointmentByIdAsync(Guid Id);
        public Task<bool> DeleteAppointmentAsync(Guid Id);
    }
}