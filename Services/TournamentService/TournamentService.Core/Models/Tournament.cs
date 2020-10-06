using Common.Core.Data;
using System;
using System.Collections.Generic;

namespace TournamentService.Core.Models
{
    public class Tournament : IEntity<int>
    {
        public Tournament()
        {
            Exercises = new HashSet<Exercise>();
            TournamentsUsers = new HashSet<TournamentsUsers>();
        }

        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? TournamentTime { get; set; }

        public virtual ICollection<Exercise> Exercises { get; set; }
        public virtual ICollection<TournamentsUsers> TournamentsUsers { get; set; }
    }
}