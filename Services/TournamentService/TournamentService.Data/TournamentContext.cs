using Microsoft.EntityFrameworkCore;
using TournamentService.Core.Models;

namespace TournamentService.Data
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>().HasKey(t => t.Id);
            modelBuilder.Entity<Tournament>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Tournament>()
                .HasMany(t => t.Exercises)
                .WithOne(e => e.Tournament)
                .HasForeignKey(e => e.TournamentId);

            modelBuilder.Entity<Exercise>().HasKey(e => e.Id);
            modelBuilder.Entity<Exercise>().Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TournamentsUsers>().HasKey(r => r.Id);
            modelBuilder.Entity<TournamentsUsers>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TournamentsUsers>()
                .HasOne(tu => tu.Tournament)
                .WithMany(t => t.TournamentsUsers)
                .HasForeignKey(tu => tu.TournamentId)
                .IsRequired();

            modelBuilder.Entity<ExercisesUsers>().HasKey(r => r.Id);
            modelBuilder.Entity<ExercisesUsers>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ExercisesUsers>()
                .HasOne(eu => eu.Exercise)
                .WithMany(e => e.ExercisesUsers)
                .HasForeignKey(eu => eu.ExerciseId)
                .IsRequired();
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<TournamentsUsers> TournamentsUsers { get; set; }
        public DbSet<ExercisesUsers> ExercisesUsers { get; set; }
    }
}