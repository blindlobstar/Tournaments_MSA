namespace ExerciseFlow.API.Models
{
    public sealed class Exercise
    {
        public int Id { get; set; }
        public int? OrderNumber { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public int TournamentId { get; set; }
    }
}
