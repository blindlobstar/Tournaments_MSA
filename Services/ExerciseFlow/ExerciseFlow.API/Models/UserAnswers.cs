using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExerciseFlow.API.Models
{
    public class UserAnswers
    {
        public int ExerciseId { get; set; }
        public string UserId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
