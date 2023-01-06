using DisprzTraining.Dto;
using DisprzTraining.Models;

namespace DisprzTraining.validation
{
    public interface IAppointmentValidation
    {
        public Task<Appointment> ExistingAppointment(AppointmentDto appointmentDto);
        // public Task<bool> ExistingAppointment(AppointmentDto appointmentDto);
        public Task<List<Appointment>> FindAppointments(string? date);
    }
}