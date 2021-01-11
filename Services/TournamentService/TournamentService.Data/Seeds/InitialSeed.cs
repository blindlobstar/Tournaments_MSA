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
            //using (var transaction = dataContext.Database.BeginTransaction())
            //{

            var tournament = new TournamentDto()
            {
                Id = 1,
                Caption = "New Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };
            var tournament2 = new TournamentDto()
            {
                Id = 2,
                Caption = "Old Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };

            if (!dataContext.Tournaments.Any())
            {
                dataContext.Tournaments.Add(tournament);
                dataContext.Tournaments.Add(tournament2);
                //dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tournaments ON");
                dataContext.SaveChanges();
                //dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tournaments OFF");
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
                    new ExerciseDto()
                    {
                        Id = 1,
                        Text = "1+1",
                        Answer = "2",
                        OrderNumber = 1,
                        Tournament = tournament
                    }, new ExerciseDto()
                    {
                        Id = 2,
                        Text = "3+5",
                        Answer = "8",
                        OrderNumber = 2,
                        Tournament = tournament
                    }, new ExerciseDto()
                    {
                        Id = 3,
                        Text = "First three letters of alphabet",
                        Answer = "a,b,c",
                        OrderNumber = 3,
                        Tournament = tournament
                    }, new ExerciseDto()
                    {
                        Id = 4,
                        Text = "First three letters of alphabet",
                        Answer = "a,b,c",
                        OrderNumber = 3,
                        Tournament = tournament2
                    });
                //dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Exercises ON");
                dataContext.SaveChanges();
                //dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Exercises OFF");
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
            //    transaction.Commit();
            //}
        }

        public static void SeedData(this TournamentContext dataContext)
        {
            dataContext.Database.EnsureCreated();


            var tournament = new TournamentDto()
            {
                Caption = "New Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };
            var tournament2 = new TournamentDto()
            {
                Caption = "Old Tournament",
                Description = "First added tournament",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                TournamentTime = 20
            };

            if (!dataContext.Tournaments.Any())
            {
                dataContext.Tournaments.Add(tournament);
                dataContext.Tournaments.Add(tournament2);
                dataContext.SaveChanges();
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
                    new ExerciseDto()
                    {
                        Text = "1+1",
                        Answer = "2",
                        OrderNumber = 1,
                        Tournament = tournament
                    }, new ExerciseDto()
                    {
                        Text = "3+5",
                        Answer = "8",
                        OrderNumber = 2,
                        Tournament = tournament
                    }, new ExerciseDto()
                    {
                        Text = "First three letters of alphabet",
                        Answer = "a,b,c",
                        OrderNumber = 3,
                        Tournament = tournament
                    }, new ExerciseDto()
                    {
                        Text = "First three letters of alphabet",
                        Answer = "a,b,c",
                        OrderNumber = 3,
                        Tournament = tournament2
                    });
                dataContext.SaveChanges();
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
        }

        public static void EnsureSeedIdentityInsert(this TournamentContext dataContext)
        {
            dataContext.Database.EnsureCreated();
            var strategy = dataContext.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = dataContext.Database.BeginTransaction();
                var tournament = new TournamentDto()
                {
                    Id = 1,
                    Caption = "New Tournament",
                    Description = "First added tournament",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(3),
                    TournamentTime = 20
                };
                var tournament2 = new TournamentDto()
                {
                    Id = 2,
                    Caption = "Old Tournament",
                    Description = "First added tournament",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(3),
                    TournamentTime = 20
                };

                if (!dataContext.Tournaments.Any())
                {
                    dataContext.Tournaments.Add(tournament);
                    dataContext.Tournaments.Add(tournament2);
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tournament ON");
                    dataContext.SaveChanges();
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Tournament OFF");
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
                        new ExerciseDto()
                        {
                            Id = 1,
                            Text = "1+1",
                            Answer = "2",
                            OrderNumber = 1,
                            Tournament = tournament
                        }, new ExerciseDto()
                        {
                            Id = 2,
                            Text = "3+5",
                            Answer = "8",
                            OrderNumber = 2,
                            Tournament = tournament
                        }, new ExerciseDto()
                        {
                            Id = 3,
                            Text = "First three letters of alphabet",
                            Answer = "a,b,c",
                            OrderNumber = 3,
                            Tournament = tournament
                        }, new ExerciseDto()
                        {
                            Id = 4,
                            Text = "First three letters of alphabet",
                            Answer = "a,b,c",
                            OrderNumber = 3,
                            Tournament = tournament2
                        });
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Exercise ON");
                    dataContext.SaveChanges();
                    dataContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Exercise OFF");
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
            });
        }
    }
}