using DisprzTraining.Dto;
using DisprzTraining.Models;
using DisprzTraining.Result;

namespace DisprzTraining.DataAccess{
    public interface IAppointmentDAL{
        public Task<ResultModel> CreateAppointmentAsync(AppointmentDto appointmentDto);
        public List<Appointment> GetAppointmentAsync(string date);
        // public Task<Appointment> GetAppointmentByIdAsync(Guid Id);
        public Task<bool> DeleteAppointmentAsync(Guid Id);
        public bool DeleteAppointment(DateTime startDateTime);
        public ResultModel UpdateAppointment(Guid Id, AppointmentDto appointmentDto);
        public Appointment? ExistingAppointment(AppointmentDto appointmentDto);
        // public List<Appointment> FindAppointments(string? date);

    }
}