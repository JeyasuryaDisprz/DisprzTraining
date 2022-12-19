using DisprzTraining.Models;
using DisprzTraining.Data;

namespace DisprzTraining.validation{
    public class AppointmentValidation:IAppointmentValidation{

        public async Task<bool> ValideDate(Appointment appoinment){
            var AppointmentsInDate = await FindAppointments(appoinment.Date);
            foreach(var appoinmentDB in AppointmentsInDate){
                if((appoinment.StartDateTime >= appoinmentDB.StartDateTime && appoinment.StartDateTime < appoinmentDB.EndDateTime) || 
                        (appoinment.EndDateTime > appoinmentDB.StartDateTime && appoinment.EndDateTime <= appoinmentDB.EndDateTime))
                {
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<List<Appointment>> FindAppointments(string? date){
            return await Task.FromResult(AppointmentData.Appointments.FindAll( x => x.Date == date ));
        }

        public async Task<Appointment> FindAppointment(Guid Id){
            return await Task.FromResult(AppointmentData.Appointments.Find( x => x.Id == Id));
        }

    }
}