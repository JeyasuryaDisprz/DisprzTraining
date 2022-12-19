using DisprzTraining.Models;
using DisprzTraining.validation;
using DisprzTraining.Business;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

namespace DisprzTraining.Tests{
    public class AppointmentValidationTest{
        
        // [Fact]
        // public async Task<bool> ValideDate(Appointment appoinment){
        //     var AppointmentsInDate = await FindAppointments(appoinment.Date);
        //     foreach(var appoinmentDB in AppointmentsInDate){
        //         if((appoinment.StartDateTime >= appoinmentDB.StartDateTime && appoinment.StartDateTime < appoinmentDB.EndDateTime) || 
        //                 (appoinment.EndDateTime > appoinmentDB.StartDateTime && appoinment.EndDateTime <= appoinmentDB.EndDateTime))
        //         {
        //             return await Task.FromResult(false);
        //         }
        //     }

        //     return await Task.FromResult(true);
        // }
    
    }
}