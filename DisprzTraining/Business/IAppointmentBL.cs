using DisprzTraining.Dto;
using DisprzTraining.Models;
using DisprzTraining.Result;

namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        public Task<ResultModel> CreateAsync(AppointmentDto appointmentDto);
        public Task<List<Appointment>> GetAsync(string date);
        public Task<Appointment> GetIdAsync(Guid Id);
        public Task<bool> DeleteAsync(Guid Id);
        public bool Delete(DateTime startDateTime);
    }
}