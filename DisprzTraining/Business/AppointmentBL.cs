using DisprzTraining.Models;
using DisprzTraining.Dto;
using DisprzTraining.DataAccess;
using DisprzTraining.Result;
using Extension;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {
        private readonly IAppointmentDAL _appointmentDAL;

        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;
        }

        public ResultModel Create(AppointmentDto appointmentDto)
        {
            switch ((int)appointmentDto.Routine)
            {
                case 1:
                    if ((int)appointmentDto.StartDateTime.DayOfWeek == 0)
                    {
                        appointmentDto.StartDateTime = appointmentDto.StartDateTime.AddDays(1);
                        appointmentDto.EndDateTime = appointmentDto.EndDateTime.AddDays(1);
                    }
                    else if ((int)appointmentDto.StartDateTime.DayOfWeek == 6)
                    {
                        appointmentDto.StartDateTime = appointmentDto.StartDateTime.AddDays(2);
                        appointmentDto.EndDateTime = appointmentDto.EndDateTime.AddDays(2);
                    }
                    var endOfWeek = appointmentDto.StartDateTime.AddDays(5 - (int)appointmentDto.StartDateTime.DayOfWeek);
                    return CreateRoutine(appointmentDto, endOfWeek);
                case 2:
                    var daysInMonth = DateTime.DaysInMonth(appointmentDto.StartDateTime.Year, appointmentDto.StartDateTime.Month);
                    var endOfMonth = appointmentDto.StartDateTime.AddDays(daysInMonth - appointmentDto.StartDateTime.Day);
                    return CreateRoutine(appointmentDto, endOfMonth);
            };
            var appointment = appointmentDto.ToAppointment(Guid.Empty);
            return _appointmentDAL.CreateSingleOrMultipleAppointments(appointment);
        }

        public List<Appointment> Get(string date)
        {
            return _appointmentDAL.GetAppointment(date);
        }

        public bool Delete(Guid Id)
        {
            return _appointmentDAL.DeleteAppointment(Id);
        }

        public ResultModel Update(Guid Id, AppointmentDto appointmentDto)
        {
            var appointment = appointmentDto.ToAppointment(Id);
            var startDate = new DateOnly(appointment.StartDateTime.Year, appointment.StartDateTime.Month, appointment.StartDateTime.Day);
            var endDate = new DateOnly(appointment.EndDateTime.Year, appointment.EndDateTime.Month, appointment.EndDateTime.Day);
            appointment.GroupId = (endDate.DayNumber - startDate.DayNumber > 0)? Guid.NewGuid(): Guid.Empty;
            return _appointmentDAL.UpdateAppointment(Id, appointment);
        }
        public bool DeleteRoutine(Guid id)
        {
            return _appointmentDAL.DeleteRoutine(id);
        }
        public ResultModel CreateRoutine(AppointmentDto appointmentDto, DateTime limit)
        {
            var groupId = Guid.NewGuid();
            var prevId = Guid.Empty;
            while (appointmentDto.StartDateTime <= limit)
            {
                if (((int)appointmentDto.StartDateTime.DayOfWeek > 0) && ((int)appointmentDto.StartDateTime.DayOfWeek < 6))
                {
                    var appointment = appointmentDto.ToAppointment(Guid.Empty);
                    var result = _appointmentDAL.CreateAppointment(appointment);

                    if (result.message != "")
                    {
                        if (prevId != Guid.Empty)
                        {
                            DeleteRoutine(prevId);
                        }
                        return result;
                    }
                    prevId = groupId;
                    appointment.GroupId = groupId;
                }
                appointmentDto.StartDateTime = appointmentDto.StartDateTime.AddDays(1);
                appointmentDto.EndDateTime = appointmentDto.EndDateTime.AddDays(1);
            }
            return new ResultModel() { id = groupId, message = "" };
        }

        public List<RoutineDto> GetRoutines()
        {
            return _appointmentDAL.GetRoutines();
        }

        public Appointment GetById(Guid id)
        {
            return _appointmentDAL.GetById(id);
        }
    }
}

// public ResultModel CreateAppointment(AppointmentDto appointmentDto)
//         {
//             var startDate = new DateOnly(appointmentDto.StartDateTime.Year, appointmentDto.StartDateTime.Month, appointmentDto.StartDateTime.Day);
//             var endDate = new DateOnly(appointmentDto.EndDateTime.Year, appointmentDto.EndDateTime.Month, appointmentDto.EndDateTime.Day);
//             var dateDifference = endDate.DayNumber - startDate.DayNumber;
//             var appointmentEndDateTime = appointmentDto.EndDateTime;
//             var groupId = Guid.NewGuid();
//             var prevId = Guid.Empty;
//             var resultId = Guid.Empty;
//             while (dateDifference >= 0)
//             {
//                 if (dateDifference < 1)
//                 {
//                     appointmentDto.EndDateTime = appointmentEndDateTime;
//                     var appointment = appointmentDto.ToAppointment(Guid.Empty);
//                     appointment.GroupId = prevId;
//                     var result = _appointmentDAL.CreateAppointment(appointment);
//                     if(result.message != "")
//                     {
//                         if(prevId != Guid.Empty)
//                         {
//                             DeleteRoutine(groupId);
//                         }
//                     }
//                     return result;
//                 }
//                 else
//                 {
//                     var date = startDate.AddDays(1);
//                     appointmentDto.EndDateTime = new DateTime(date.Year,date.Month,date.Day);
//                     var appointment = appointmentDto.ToAppointment(Guid.Empty);
//                     appointment.GroupId = groupId;
//                     prevId = groupId;
//                     var result = _appointmentDAL.CreateAppointment(appointment);
//                     if(result.message != "")
//                     {
//                         if(prevId != Guid.Empty)
//                         {
//                             DeleteRoutine(groupId);
//                         }
//                         return result;
//                     }
//                     resultId = result.id;
//                     appointmentDto.StartDateTime = appointmentDto.EndDateTime;
//                 }
//                 startDate = new DateOnly(appointmentDto.StartDateTime.Year,appointmentDto.StartDateTime.Month,appointmentDto.StartDateTime.Day);
//                 dateDifference = endDate.DayNumber - startDate.DayNumber;
//             }
//             return new ResultModel() {id = resultId, message = "" };   
//         }