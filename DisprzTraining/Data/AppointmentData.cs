using DisprzTraining.Models;
namespace DisprzTraining.Data{
    public class AppointmentData{
        public static List<Appointment> Appointments = new(){
            new Appointment(){
                Id = new Guid("3fa85f64-5717-9999-b3fc-2c963f66afa6"),
                StartDateTime = new DateTime(2022,1,1,03,00,00),
                EndDateTime = new DateTime(2022,1,1,04,00,00),
                Title = "Test Appointment 1"
            },
            new Appointment(){
                Id = new Guid("3fa85f64-7777-9999-b3fc-2c963f66afa6"),
                StartDateTime = new DateTime(2022,1,1,07,00,00),
                EndDateTime = new DateTime(2022,1,1,10,00,00),
                Title = "Test Appointment 1"
            }
        };
    }
}