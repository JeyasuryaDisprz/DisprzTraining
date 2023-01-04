using DisprzTraining.Models;

namespace DisprzTraining.Result{
    
    public class ResultModel{
        
        public int ResultStatusCode{get; set;}
        public string? Message{get; set;}
        public Appointment appointment = new();
    }
}