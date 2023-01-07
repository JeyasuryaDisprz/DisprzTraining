using DisprzTraining.Dto;
using DisprzTraining.Models;

namespace DisprzTraining.validation
{
    public interface IAppointmentValidation
    {
<<<<<<< HEAD
        // public Task<Appointment> ExistingAppointment(AppointmentDto appointmentDto);
        // // public Task<bool> ExistingAppointment(AppointmentDto appointmentDto);
        // public Task<List<Appointment>> FindAppointments(string? date);
=======
        public Task<Appointment> ExistingAppointment(AppointmentDto appointmentDto);
        // public Task<bool> ExistingAppointment(AppointmentDto appointmentDto);
        public Task<List<Appointment>> FindAppointments(string? date);
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
    }
}