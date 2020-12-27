using System.Collections.Generic;
using Common.Core.Data;

namespace TournamentService.Core.Models
{
    public class ExerciseDto : IEntity<int>
    {
        public ExerciseDto()
        {
            ExercisesUsers = new HashSet<ExercisesUsers>();
        }

        public int Id { get; set; }
        public int? OrderNumber { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public int TournamentId { get; set; }

        public virtual TournamentDto Tournament { get; set; }
        public virtual ICollection<ExercisesUsers> ExercisesUsers { get; set; }
    }
}