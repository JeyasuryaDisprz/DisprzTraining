using DisprzTraining.Models;
using DisprzTraining.validation;
using DisprzTraining.Data;
using DisprzTraining.Dto;
using System.Globalization;
using DisprzTraining.Result;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private static List<Appointment> Appointments = new() { };

        public async Task<ResultModel> CreateAppointmentAsync(AppointmentDto appointmentDto)
        {
            Appointment? result = ExistingAppointment(appointmentDto);

            if (result is null)
            {
                var appointment = new Appointment()
                {
                    Id = Guid.NewGuid(),
                    StartDateTime = appointmentDto.StartDateTime,
                    EndDateTime = appointmentDto.EndDateTime,
                    Title = appointmentDto.Title,
                    Description = appointmentDto.Description
                };

                Appointments.Add(appointment);
                Appointments = Appointments.Where(appointment => appointment != null).OrderBy(appointment => appointment.StartDateTime).ToList();
                return new ResultModel() { appointmentId = appointment.Id, ErrorMessage = "" };
            }
            else
            {
                return new ResultModel() { ErrorMessage = $"Meet already found between {result.StartDateTime.ToString("hh:mm:tt")} - {result.EndDateTime.ToString("hh:mm:tt")}" };
            }
        }

        public List<Appointment> GetAppointmentAsync(string date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return
                (from appointment in Appointments
                 where appointment.StartDateTime >= dateFormatted && appointment.StartDateTime < dateFormatted.AddDays(1)
                 select appointment).ToList();
        }

        public async Task<bool> DeleteAppointmentAsync(Guid Id)
        {
            var appoinment = Appointments.Where(appoinment => appoinment.Id == Id).SingleOrDefault();

            if (appoinment is not null)
            {
                Appointments.Remove((Appointment)appoinment);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public bool DeleteAppointment(DateTime startDateTime)
        {
            var index = BinarySearchAppointments(startDateTime);

            if (index != -1)
            {
                Appointments.RemoveAt(index);
                return true;
            }
            return false;
        }

        public ResultModel UpdateAppointment(Guid Id, AppointmentDto appointmentDto)
        {
            if (Appointments.Exists(appointment => appointment.Id == Id))
            {
                Appointment? result = ExistingAppointment(Id, appointmentDto);

                if (result is null)
                {
                    var UpdateAppointment = (from appointment in Appointments where appointment.Id == Id select appointment).FirstOrDefault() as Appointment;
                    UpdateAppointment.StartDateTime = appointmentDto.StartDateTime;
                    UpdateAppointment.EndDateTime = appointmentDto.EndDateTime;
                    UpdateAppointment.Title = appointmentDto.Title;
                    UpdateAppointment.Description = appointmentDto.Description;

                    Appointments = Appointments.Where(appointment => appointment != null).OrderBy(appointment => appointment.StartDateTime).ToList();
                    return new ResultModel() { appointmentId = Id, ErrorMessage = "" };
                }
                else
                {
                    return new ResultModel() { appointmentId = result.Id, ErrorMessage = $"Cannot set, Meeting already found at between {result.StartDateTime.ToString("hh:mm:tt")} - {result.EndDateTime.ToString("hh:mm:tt")}" };
                }
            }
            else
            {
                return new ResultModel(){
                    appointmentId = Guid.Empty,
                    ErrorMessage = $"Meeting not found at Id: {Id}"
                };
            }
        }

        public Appointment? ExistingAppointment(AppointmentDto appointmentDto)
        {
            var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");

            var AppointmentsInDate = GetAppointmentAsync(stringDate);
            var filterAppointments =
                (from appointment in AppointmentsInDate
                 where appointment.StartDateTime < appointmentDto.EndDateTime && appointment.EndDateTime > appointmentDto.StartDateTime
                 select appointment).FirstOrDefault() as Appointment;
            return filterAppointments;
        }
        public Appointment? ExistingAppointment(Guid Id, AppointmentDto appointmentDto)
        {
            var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");

            var AppointmentsInDate = GetAppointmentAsync(stringDate);
            var filterAppointments =
                (from appointment in AppointmentsInDate
                 where appointment.Id != Id && (appointment.StartDateTime < appointmentDto.EndDateTime && appointment.EndDateTime > appointmentDto.StartDateTime)
                 select appointment).FirstOrDefault() as Appointment;
            return filterAppointments;
        }

        // public List<Appointment> FindAppointments(string? date)
        // {
        //     DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //     return Appointments.FindAll(x => x.StartDateTime >= dateFormatted && x.StartDateTime < dateFormatted.AddDays(1));
        // }

        public void TestList(Appointment appointment)
        {
            Appointments.Add(appointment);
        }

        public int BinarySearchAppointments(DateTime startDateTime)
        {
            int start = 0, end = Appointments.Count() - 1;

            while (start <= end)
            {
                int mid = (start + end) / 2;
                var checkedValue = DateTime.Compare(startDateTime, Appointments[mid].StartDateTime);
                if (checkedValue == 0)
                {
                    return mid;
                }
                else if (checkedValue < 0)
                {
                    end = mid - 1;
                }
                else
                {
                    start = mid + 1;
                }
            }
            return -1;
        }
    }
}