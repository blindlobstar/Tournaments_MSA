using System.Linq;
using System;
using TournamentService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace TournamentService.Data.Seeds
{
    public static class InitialSeed
    {
        public static void EnsureSeed(this TournamentContext dataContext)
        {
            dataContext.Database.EnsureCreated();
            using (var transaction = dataContext.Database.BeginTransaction())
            {

                var tournament = new Tournament()
                {
                    Id = 1,
                    Caption = "New Tournament",
                    Description = "First added tournament",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(3),
                    TournamentTime = 20
                };

                if (!dataContext.Tournaments.Any())
                {
                    dataContext.Tournaments.Add(tournament);
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tournaments ON");
                    dataContext.SaveChanges();
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tournaments OFF");
                }

                if (!dataContext.TournamentsUsers.Any())
                {
                    dataContext.TournamentsUsers.Add(new TournamentsUsers()
                    {
                        Tournament = tournament,
                        UserId = "5e7398bde6ab1940182c5cfd"
                    });
                    dataContext.SaveChanges();
                }

                if (!dataContext.Exercises.Any())
                {
                    dataContext.Exercises.AddRange(
                        new Exercise()
                        {
                            Id = 1,
                            Text = "1+1",
                            Answer = "2",
                            OrderNumber = 1,
                            Tournament = tournament
                        }, new Exercise()
                        {
                            Id = 2,
                            Text = "3+5",
                            Answer = "8",
                            OrderNumber = 2,
                            Tournament = tournament
                        }, new Exercise()
                        {
                            Id = 3,
                            Text = "First three letters of alphabet",
                            Answer = "a,b,c",
                            OrderNumber = 3,
                            Tournament = tournament
                        });
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Exercises ON");
                    dataContext.SaveChanges();
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Exercises OFF");
                }

                if (!dataContext.ExercisesUsers.Any())
                {
                    dataContext.ExercisesUsers.AddRange(
                        new ExercisesUsers()
                        {
                            ExerciseId = 1,
                            UserId = "5e7398bde6ab1940182c5cfd",
                            UserAnswer = "2",
                            IsCorrect = true
                        });
                    dataContext.SaveChanges();
                }
                dataContext.SaveChanges();
                transaction.Commit();
            }
        }
    }
}