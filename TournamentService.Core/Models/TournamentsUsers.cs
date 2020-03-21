using Common.Core.Data;

namespace TournamentService.Core.Models
{
    public class TournamentsUsers : IEntity<int>
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string UserId { get; set; }
        public uint? Place { get; set; }
        public virtual Tournament Tournament { get; set; }
    }
}