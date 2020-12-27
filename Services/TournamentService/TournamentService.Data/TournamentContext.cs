using Microsoft.EntityFrameworkCore;
using TournamentService.Core.Models;

namespace TournamentService.Data
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TournamentDto>().ToTable("Tournament");
            modelBuilder.Entity<TournamentDto>().HasKey(t => t.Id);
            modelBuilder.Entity<TournamentDto>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TournamentDto>()
                .HasMany(t => t.Exercises)
                .WithOne(e => e.Tournament)
                .HasForeignKey(e => e.TournamentId);

            modelBuilder.Entity<ExerciseDto>().ToTable("Exercise");
            modelBuilder.Entity<ExerciseDto>().HasKey(e => e.Id);
            modelBuilder.Entity<ExerciseDto>().Property(t => t.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<TournamentsUsers>().ToTable("TournamentsUsers");
            modelBuilder.Entity<TournamentsUsers>().HasKey(r => r.Id);
            modelBuilder.Entity<TournamentsUsers>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TournamentsUsers>()
                .HasOne(tu => tu.Tournament)
                .WithMany(t => t.TournamentsUsers)
                .HasForeignKey(tu => tu.TournamentId)
                .IsRequired();

            modelBuilder.Entity<ExercisesUsers>().ToTable("ExercisesUsers");
            modelBuilder.Entity<ExercisesUsers>().HasKey(r => r.Id);
            modelBuilder.Entity<ExercisesUsers>().Property(t => t.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ExercisesUsers>()
                .HasOne(eu => eu.Exercise)
                .WithMany(e => e.ExercisesUsers)
                .HasForeignKey(eu => eu.ExerciseId)
                .IsRequired();
        }

        public DbSet<TournamentDto> Tournaments { get; set; }
        public DbSet<ExerciseDto> Exercises { get; set; }
        public DbSet<TournamentsUsers> TournamentsUsers { get; set; }
        public DbSet<ExercisesUsers> ExercisesUsers { get; set; }
    }
}