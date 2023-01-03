using DisprzTraining.Dto;
using DisprzTraining.Models;

namespace DisprzTraining.DataAccess{
    public interface IAppointmentDAL{
        public Task<bool> CreateAppointmentAsync(AppointmentDto appointmentDto);
        public Task<List<Appointment>> GetAppointmentAsync(DateTime date);
        public Task<bool> DeleteAppointmentAsync(Guid Id);
    }
}