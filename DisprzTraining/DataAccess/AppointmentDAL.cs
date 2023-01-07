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
<<<<<<< HEAD
        private static List<Appointment> Appointments = new() { };
        // private readonly IAppointmentValidation _appointmentValidation;
=======
        private readonly IAppointmentValidation _appointmentValidation;
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912

        // public AppointmentDAL(IAppointmentValidation appointmentValidation)
        // {
        //     _appointmentValidation = appointmentValidation;
        // }

        public async Task<ResultModel> CreateAppointmentAsync(AppointmentDto appointmentDto)
        {
<<<<<<< HEAD
            Appointment? result = ExistingAppointment(appointmentDto);
=======
            Appointment result = await _appointmentValidation.ExistingAppointment(appointmentDto);
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
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

<<<<<<< HEAD
                Appointments.Add(appointment);
                Appointments = Appointments.Where(appointment => appointment != null).OrderBy(appointment => appointment.StartDateTime).ToList();
                return new ResultModel() { appointmentId = appointment.Id, ErrorMessage = "" };
            }
            else
            {
                return new ResultModel() { ErrorMessage = $"Meet already found between {result.StartDateTime.ToString("hh:mm:tt")} - {result.EndDateTime.ToString("hh:mm:tt")}" };
=======
                AppointmentData.Appointments.Add(appointment);
                return new ResultModel(){ResultStatusCode = 201, appointment = appointment};
            }
            else
            {
                return new ResultModel(){ResultStatusCode = 409, appointment = result};
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
            }
        }

        public async Task<List<Appointment>> GetAppointmentAsync(string date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

<<<<<<< HEAD
            return
                (from appointment in Appointments
                 where appointment.StartDateTime >= dateFormatted && appointment.EndDateTime < dateFormatted.AddDays(1)
                 select appointment).ToList();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid Id)
        {
            return
                (from appointment in Appointments
                 where appointment.Id == Id
                 select appointment).FirstOrDefault() as Appointment;
        }
        public async Task<bool> DeleteAppointmentAsync(Guid Id)
        {
            var appoinment = Appointments.Where(appoinment => appoinment.Id == Id).SingleOrDefault();

            if (appoinment is not null)
            {
                Appointments.Remove((Appointment)appoinment);
=======
            return 
                (from appointment in AppointmentData.Appointments
                where appointment.StartDateTime >= dateFormatted && appointment.EndDateTime < dateFormatted.AddDays(1)
                orderby appointment.StartDateTime ascending
                select appointment).ToList();

        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid Id){
            return
                (from appointment in AppointmentData.Appointments
                where appointment.Id == Id
                select appointment).FirstOrDefault() as Appointment;
        }
        public async Task<bool> DeleteAppointmentAsync(Guid Id)
        {
            var appoinment = AppointmentData.Appointments.Where(appoinment => appoinment.Id == Id).SingleOrDefault();

            if (appoinment is not null)
            {
                AppointmentData.Appointments.Remove((Appointment)appoinment);
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
        public bool DeleteAppointment(DateTime startDateTime)
        {
            var index = BinarySearchAppointments(startDateTime);

<<<<<<< HEAD
            if (index != -1)
            {
                Appointments.RemoveAt(index);
                return true;
            }
            return false;
        }

        public Appointment? ExistingAppointment(AppointmentDto appointmentDto)
        {
            var dateString = DateOnly.FromDateTime((DateTime)appointmentDto.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");

            var AppointmentsInDate = FindAppointments(stringDate);
            var filterAppointments =
                (from appointment in AppointmentsInDate
                 where appointment.StartDateTime < appointmentDto.EndDateTime && appointment.EndDateTime > appointmentDto.StartDateTime
                 select appointment).FirstOrDefault() as Appointment;
            return filterAppointments;
        }

        public List<Appointment> FindAppointments(string? date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return Appointments.FindAll(x => x.StartDateTime >= dateFormatted && x.StartDateTime < dateFormatted.AddDays(1));
        }

        public void TestList(Appointment appointment)
        {
            Appointments.Add(appointment);
        }






        public int BinarySearchAppointments(DateTime startDateTime)
        {
            int start = 0, end = Appointments.Count() - 1;

            while(start <= end){
                int mid = (start+end)/2;
                var checkedValue = DateTime.Compare(startDateTime,Appointments[mid].StartDateTime);
                if(checkedValue == 0){
                    return mid;
                }
                else if(checkedValue < 0){
                    end = mid -1;
                }
                else{
                    start = mid+1;
                }
            }
            return -1;
        }
=======
        // public bool ExistingAppointments(AppointmentDto appointmentDto){
        //     foreach (var appointmentDB in Appointments)
        //     {
        //         if ((appointmentDto.StartDateTime >= appointmentDB.StartDateTime && appointmentDto.StartDateTime < appointmentDB.EndDateTime) ||
        //                 (appointmentDto.EndDateTime > appointmentDB.StartDateTime && appointmentDto.EndDateTime <= appointmentDB.EndDateTime))
        //         {
        //             return false;
        //         }
        //     }

        //     return true;
        // }
>>>>>>> d8716df5631ee5b0d3fedee0172a6d3928167912
    }
}