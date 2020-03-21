using System.ComponentModel.DataAnnotations;
using Common.Core.Data;

namespace TournamentService.Core.Models
{
    public class ExercisesUsers : IEntity<int>
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        [Required]
        public string UserId { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }

        public virtual Exercise Exercise { get; set; }
    }
}