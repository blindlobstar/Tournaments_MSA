using System.Diagnostics;
using Akka.Actor;

namespace ExerciseFlow.API.Actors
{
    public sealed class ExerciseActor : UntypedActor
    {
        public interface IAnswerResponse
        {
            
        }

        public sealed class GetExerciseRequest
        {
            public GetExerciseRequest(int requestId)
            {
                RequestId = requestId;
            }

            public int RequestId { get; }
        }

        public sealed class GetExerciseResponse
        {
            public GetExerciseResponse(string question, int time)
            {
                Question = question;
                Time = time;
            }

            public string Question { get; }
            public int Time { get; }
        }

        public sealed class TakeAnswerRequest
        {
            public TakeAnswerRequest(string answer)
            {
                Answer = answer;
            }

            public string Answer { get; }
        }

        public sealed class TakeAnswerResponse : IAnswerResponse
        {
            public TakeAnswerResponse(int exerciseId, bool isCorrect, string answer)
            {
                ExerciseId = exerciseId;
                IsCorrect = isCorrect;
                Answer = answer;
            }

            public int ExerciseId { get; }
            public bool IsCorrect { get; }
            public string Answer { get; }
        }

        public sealed class ExerciseNotInUse
        {
            public ExerciseNotInUse(int exerciseId)
            {
                ExerciseId = exerciseId;
            }

            public int ExerciseId { get; }
        }

        private readonly int _exerciseId;
        private readonly string _question;
        private readonly string _answer;
        private readonly int _time;
        private readonly Stopwatch _timer;

        public ExerciseActor(int time, string answer, string question, int exerciseId)
        {
            _time = time;
            _answer = answer;
            _question = question;
            _exerciseId = exerciseId;
            _timer = new Stopwatch();
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case GetExerciseRequest:
                    if(!_timer.IsRunning)
                        _timer.Start();

                    Sender.Tell(new GetExerciseResponse(_question, _time));
                    break;
                case TakeAnswerRequest request:
                    if (!_timer.IsRunning)
                    {
                        Sender.Tell(new ExerciseNotInUse(_exerciseId));
                        break;
                    }
                    
                    _timer.Stop();
                    var elapsed = _timer.Elapsed.Seconds;
                    if (elapsed > _time || !IsAnswerCorrect(request.Answer))
                    {
                        Sender.Tell(new TakeAnswerResponse(_exerciseId, false, request.Answer));
                        Context.Stop(Self);
                        break;
                    }
                    
                    Sender.Tell(new TakeAnswerResponse(_exerciseId, true, request.Answer));
                    Context.Stop(Self);
                    break;
            }
        }

        private bool IsAnswerCorrect(string answer) => answer == _answer;

        public static Props Props(int exerciseId, string question, string answer, int time) =>
            Akka.Actor.Props.Create(() => new ExerciseActor(time, answer, question, exerciseId));
    }
}
