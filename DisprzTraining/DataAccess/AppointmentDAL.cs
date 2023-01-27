using DisprzTraining.Models;
using System.Globalization;
using DisprzTraining.Result;
using DisprzTraining.Dto;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private static Dictionary<Guid, Appointment> appointments = new();

        public ResultModel CreateAppointment(Appointment appointment)
        {
            var result = ExistingAppointment(appointment);
            if (result is null)
            {
                appointments.Add(appointment.Id, appointment);
                return new ResultModel() { id = appointment.Id, message = "" };
            }

            return new ResultModel() { message = $"Meeting already found between {result.StartDateTime.ToString("hh:mm tt")} and {result.EndDateTime.ToString("hh:mm tt")} on {result.StartDateTime.ToString("dd-MM-yyyy")}" };
        }

        public List<Appointment> GetAppointment(string date)
        {
            DateTime dateFormatted = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime dateFormattedLimit = dateFormatted.AddDays(1);

            return (
                from appointment in appointments
                where appointment.Value.StartDateTime >= dateFormatted && appointment.Value.StartDateTime < dateFormattedLimit
                select appointment.Value).ToList();
        }

        public bool DeleteAppointment(Guid Id)
        {
            if (appointments.ContainsKey(Id))
            {
                if (appointments[Id].GroupId != Guid.Empty && appointments[Id].Routine == Routine.None)
                {
                    return DeleteRoutine(appointments[Id].GroupId);
                }
                return appointments.Remove(Id);
            }
            return false;
        }

        public ResultModel UpdateAppointment(Guid Id, Appointment appointment)
        {
            if (appointments.ContainsKey(Id))
            {
                appointment.GroupId = appointments[Id].GroupId;
                Appointment? result = ExistingAppointment(appointment);
                if (result is null)
                {
                    if (appointments[Id].GroupId != Guid.Empty)
                    {
                        DeleteAppointment(Id);
                        return CreateSingleOrMultipleAppointments(appointment);
                    }

                    appointment.Id = Id;
                    appointments[Id] = appointment;
                    return new ResultModel() { id = appointment.Id, message = "" };
                }
                return new ResultModel() { id = result.Id, message = $"Cannot set, Meeting already found between {result.StartDateTime.ToString("hh:mm:tt")} - {result.EndDateTime.ToString("hh:mm:tt")} on {result.StartDateTime.ToString("dd-MM-yyyy")}" };
            }

            return new ResultModel() { message = $"Meeting not found at Id: {Id}" };
        }

        public Appointment? ExistingAppointment(Appointment appointment)
        {
            var dateString = DateOnly.FromDateTime((DateTime)appointment.StartDateTime);
            var stringDate = dateString.ToString("yyyy/MM/dd");

            var appointmentsInDate = GetAppointment(stringDate);

            if (appointment.GroupId == Guid.Empty)
            {
                return (from appointmentInList in appointmentsInDate
                        where appointmentInList.Id != appointment.Id && (appointmentInList.StartDateTime < appointment.EndDateTime && appointmentInList.EndDateTime > appointment.StartDateTime)
                        select appointmentInList).FirstOrDefault() as Appointment;
            }

            return (from appointmentInList in appointmentsInDate
                    where appointmentInList.GroupId != appointment.GroupId && (appointmentInList.StartDateTime < appointment.EndDateTime && appointmentInList.EndDateTime > appointment.StartDateTime)
                    select appointmentInList).FirstOrDefault() as Appointment;
        }
        public List<RoutineDto> GetRoutines()
        {
            var routines = (
                from appointment in appointments
                where appointments[appointment.Key].GroupId != Guid.Empty
                select appointment.Value).ToList();

            var query = routines.GroupBy(routine => routine.GroupId);
            var routineList = new List<RoutineDto>();
            foreach (var routineGroup in query)
            {
                var appointment = routineGroup.Select(x => x).First();
                if (appointment.Routine != Routine.None)
                {
                    RoutineDto routine = new()
                    {
                        Title = appointment.Title,
                        Id = appointment.GroupId,
                        StartTime = appointment.StartDateTime.TimeOfDay,
                        EndTime = appointment.EndDateTime.TimeOfDay
                    };
                    routineList.Add(routine);
                }
            }
            return routineList;
        }
        public bool DeleteRoutine(Guid id)
        {
            var deleteCount = 0;
            foreach (var appointment in appointments)
            {
                if (appointment.Value.GroupId == id)
                {
                    appointments.Remove(appointment.Key);
                    deleteCount++;
                }
            }
            return (deleteCount > 0);
        }
        public void TestList(Appointment appointment)
        {
            appointments.Add(appointment.Id, appointment);
        }

        public Appointment GetById(Guid id)
        {
            return (appointments.ContainsKey(id)) ? appointments[id] : new Appointment() { Id = Guid.Empty };
        }

        public ResultModel CreateSingleOrMultipleAppointments(Appointment appointment)
        {
            var startDate = new DateOnly(appointment.StartDateTime.Year, appointment.StartDateTime.Month, appointment.StartDateTime.Day);
            var endDate = new DateOnly(appointment.EndDateTime.Year, appointment.EndDateTime.Month, appointment.EndDateTime.Day);
            var appointmentEndDateTime = appointment.EndDateTime;
            var groupId = Guid.NewGuid();
            var prevId = Guid.Empty;
            var resultId = Guid.Empty;
            var result = new ResultModel();
            while (endDate.DayNumber - startDate.DayNumber > 0)
            {
                var appointmentInUse = new Appointment()
                {
                    Id = Guid.NewGuid(),
                    Title = appointment.Title,
                    StartDateTime = appointment.StartDateTime,
                    EndDateTime = appointment.EndDateTime,
                    GroupId = appointment.GroupId,
                    Routine = appointment.Routine
                };

                var date = startDate.AddDays(1);
                appointmentInUse.EndDateTime = new DateTime(date.Year, date.Month, date.Day);
                appointmentInUse.GroupId = appointment.GroupId = prevId = groupId;
                result = CreateAppointment(appointmentInUse);
                if (result.message != "")
                {
                    if (prevId != Guid.Empty)
                    {
                        DeleteRoutine(groupId);
                    }
                    return result;
                }
                resultId = result.id;
                appointment.StartDateTime = appointment.EndDateTime = appointmentInUse.EndDateTime;
                startDate = new DateOnly(appointment.StartDateTime.Year, appointment.StartDateTime.Month, appointment.StartDateTime.Day);
            }
            appointment.EndDateTime = appointmentEndDateTime;
            result = CreateAppointment(appointment);
            if (result.message != "")
            {
                if (prevId != Guid.Empty)
                {
                    DeleteRoutine(groupId);
                }
            }
            else if (prevId == Guid.Empty)
            {
                appointment.GroupId = Guid.Empty;
            }
            return result;
        }
    }
}