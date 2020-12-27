using System;
using System.ComponentModel.DataAnnotations;
using Common.Core.Data;

namespace TournamentService.Core.Models
{
    public class TournamentsUsers : IEntity<int>
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        [Required]
        public string UserId { get; set; }
        public uint? Place { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual TournamentDto Tournament { get; set; }
    }
}