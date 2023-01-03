using DisprzTraining.Models;
using DisprzTraining.Data;
using DisprzTraining.validation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web;
using System.Globalization;

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

            // DateTime.Today.

            try
            {
                bool result = await _appointmentValidation.ValideDate(appointment);
                if (result)
                {
                    AppointmentData.Appointments.Add(appointment);
                    return true;
                }
            }
            catch(Exception e){
                throw new Exception(e.Message);
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
            // return await Task.FromResult(appoinment);
            return appoinment;
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            var deleteAppointment = new Appointment();
            deleteAppointment = await _appointmentValidation.FindAppointment(Id);

            if (deleteAppointment.Title != "")
            {
                AppointmentData.Appointments.Remove(deleteAppointment);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}