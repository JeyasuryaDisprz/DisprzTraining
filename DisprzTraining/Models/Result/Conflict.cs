using DisprzTraining.Models;

namespace DisprzTraining.Result{
    public class ResultConflict{
        public int statusCode = 409;
        public Appointment appointment = new();
    }
}