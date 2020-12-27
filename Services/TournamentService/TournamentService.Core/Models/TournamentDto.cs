using Common.Core.Data;
using System;
using System.Collections.Generic;

namespace TournamentService.Core.Models
{
    public class TournamentDto : IEntity<int>
    {
        public TournamentDto()
        {
            Exercises = new HashSet<ExerciseDto>();
            TournamentsUsers = new HashSet<TournamentsUsers>();
        }

        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TournamentTime { get; set; }

        public virtual ICollection<ExerciseDto> Exercises { get; set; }
        public virtual ICollection<TournamentsUsers> TournamentsUsers { get; set; }
    }
}