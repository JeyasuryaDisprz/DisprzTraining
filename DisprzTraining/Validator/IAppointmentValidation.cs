using DisprzTraining.Models;

namespace DisprzTraining.validation
{
    public interface IAppointmentValidation
    {
        public Task<bool> ValideDate(Appointment appointment);
        public Task<List<Appointment>> FindAppointments(string? date);
        public Task<Appointment> FindAppointment(Guid Id);
    }
}