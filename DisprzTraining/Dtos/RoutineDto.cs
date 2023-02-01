namespace DisprzTraining.Dto
{
    public class RoutineDto
    {
        public string? Title{get; set;}
        public Guid Id{get; set;}
        public TimeSpan StartTime{get; set;}
        public TimeSpan EndTime{get; set;}
    }
}