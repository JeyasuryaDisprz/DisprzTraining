using DisprzTraining.Models;
using DisprzTraining.Data;
using DisprzTraining.validation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {

        private readonly IAppointmentValidation _appointmentValidation;

        public AppointmentBL(IAppointmentValidation appointmentValidation)
        {
            _appointmentValidation = appointmentValidation;
        }

        public async Task<bool> CreateAsync(Appointment appointment)
        {
            if (await _appointmentValidation.ValideDate(appointment))
            {
                AppointmentData.Appointments.Add(appointment);
                return true;
            }
            return false;
        }

        public async Task<List<Appointment>> GetAsync(string? date)
        {
            var appointmentListInDate = await _appointmentValidation.FindAppointments(date);
            appointmentListInDate = appointmentListInDate.OrderBy(x => x.StartDateTime).ToList();

            if (appointmentListInDate.Count > 0)
            {
                return appointmentListInDate;
            }
            return new List<Appointment>();
        }
        public async Task<Appointment> GetIdAsync(Guid Id)
        {
            var appoinment = new Appointment();
            appoinment = await _appointmentValidation.FindAppointment(Id);
            return await Task.FromResult(appoinment);
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var deleteAppointment = new Appointment();
            deleteAppointment = await _appointmentValidation.FindAppointment(Id);

            if (deleteAppointment != null)
            {
                AppointmentData.Appointments.Remove(deleteAppointment);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateAsync(Appointment appointment)
        {

            throw new NotImplementedException();
        }
    }
}