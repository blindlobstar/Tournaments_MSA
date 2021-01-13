using System;
using System.Threading;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using ExerciseFlow.API.Actors;
using NUnit.Framework;

namespace ExerciseFlow.UnitTests.ActorsTest
{
    [TestFixture]
    public class ExerciseActorTest : TestKit
    {
        [Test]
        public void Must_Return_Exercise()
        {
            //Assert
            var actor = Sys.ActorOf(ExerciseActor.Props(1, "1+1", "2", 2), "exercise");
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new ExerciseActor.GetExerciseRequest(1), probe);
            var response = probe.ExpectMsg<ExerciseActor.GetExerciseResponse>();

            //Assert
            Assert.AreEqual("1+1", response.Question);
            Assert.AreEqual(2, response.Time);
        }

        [Test]
        public void Must_Return_TakeAnswerResponse_Correct_True()
        {
            //Assert
            var actor = Sys.ActorOf(ExerciseActor.Props(1, "1+1", "2", 2));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new ExerciseActor.GetExerciseRequest(1), probe);
            actor.Tell(new ExerciseActor.TakeAnswerRequest("2"), probe);
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>();
            var takeAnswerResponse = probe.ExpectMsg<ExerciseActor.TakeAnswerResponse>();
            
            //Assert
            Assert.AreEqual(1,takeAnswerResponse.ExerciseId);
            Assert.IsTrue(takeAnswerResponse.IsCorrect);
        }

        [Test]
        public void Must_Return_TakeAnswerResponse_Correct_False()
        {
            //Assert
            var actor = Sys.ActorOf(ExerciseActor.Props(1, "1+1", "2", 2));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new ExerciseActor.GetExerciseRequest(1), probe);
            actor.Tell(new ExerciseActor.TakeAnswerRequest("3"), probe);
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>();
            var takeAnswerResponse = probe.ExpectMsg<ExerciseActor.TakeAnswerResponse>();

            //Assert
            Assert.AreEqual(1, takeAnswerResponse.ExerciseId);
            Assert.IsFalse(takeAnswerResponse.IsCorrect);
        }

        [Test]
        public void Wait_2_sec_Must_Return_TakeAnswerResponse_Correct_False()
        {
            //Assert
            var actor = Sys.ActorOf(ExerciseActor.Props(1, "1+1", "2", 1));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new ExerciseActor.GetExerciseRequest(1), probe);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            actor.Tell(new ExerciseActor.TakeAnswerRequest("3"), probe);
            probe.ExpectMsg<ExerciseActor.GetExerciseResponse>();
            var takeAnswerResponse = probe.ExpectMsg<ExerciseActor.TakeAnswerResponse>();

            //Assert
            Assert.AreEqual(1, takeAnswerResponse.ExerciseId);
            Assert.IsFalse(takeAnswerResponse.IsCorrect);
        }

        [Test]
        public void Must_Return_ExerciseNotInUse()
        {
            //Assert
            var actor = Sys.ActorOf(ExerciseActor.Props(1, "1+1", "2", 1));
            var probe = CreateTestProbe();

            //Act
            actor.Tell(new ExerciseActor.TakeAnswerRequest("2"), probe);
            var response = probe.ExpectMsg<ExerciseActor.ExerciseNotInUse>();

            //Assert
            Assert.AreEqual(1, response.ExerciseId);
        }
    }
}
