using System.Collections.Generic;
using ExerciseFlow.API.Models;

namespace ExerciseFlow.API.Services
{
    public interface ITournamentService
    {
        List<Exercise> GetExercises(int tournamentId);
    }
}