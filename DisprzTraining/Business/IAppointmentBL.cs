using System.Threading.Tasks;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        // Task<HelloWorld> SayHelloWorld();
        public Task<bool> CreateAsync(Appointment appointment);
        public Task<List<Appointment>> GetAsync(string date);
        public Task<bool> DeleteAsync(int Id);

    }
}