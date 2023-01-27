using DisprzTraining.Dto;
using DisprzTraining.Models;
using DisprzTraining.Result;

namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {

        public ResultModel Create(AppointmentDto appointmentDto);
        public List<Appointment> Get(string date);
        public bool Delete(Guid Id);
        public ResultModel Update(Guid Id, AppointmentDto appointmentDto);
        // public ResultModel CreateRoutine(AppointmentDto appointmentDto);
        public ResultModel CreateRoutine(AppointmentDto appointmentDto, DateTime limit);
        public List<RoutineDto> GetRoutines();
        // public List<Dictionary<Guid,Appointment>> GetAllRoutine();
        public bool DeleteRoutine(Guid id);
        public Appointment GetById(Guid id);
    }
}