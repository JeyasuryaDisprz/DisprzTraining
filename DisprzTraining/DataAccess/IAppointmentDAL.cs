using DisprzTraining.Dto;
using DisprzTraining.Models;
using DisprzTraining.Result;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        public ResultModel CreateAppointment(Appointment appointment);
        public List<Appointment> GetAppointment(string date);
        public bool DeleteAppointment(Guid Id);
        public ResultModel UpdateAppointment(Guid Id, Appointment appointment);
        public void TestList(Appointment appointment);
        public Appointment ExistingAppointment(Appointment appointment);
        public List<RoutineDto> GetRoutines();
        public bool DeleteRoutine(Guid id);
        public Appointment GetById(Guid id);
        public ResultModel CreateSingleOrMultipleAppointments(Appointment appointment);
    }
}