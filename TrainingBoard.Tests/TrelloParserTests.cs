using System;
using NUnit.Framework;
using Rhino.Mocks;
using TrainingBoard.TrelloIntegrator;

namespace TrainingBoard.Tests
{
    [TestFixture]
    public class TrelloParserTests
    {
        [Test]
        public void fetch_participants_for_next_day()
        {
            // Arrange
            var clock = MockRepository.GenerateMock<IClock>();
            clock.Stub(t => t.Now).Return(new DateTime(2013, 10, 12));
            var f = new FetchUp(clock);
            // Act
            var nextup = f.GetUpcoming();
            // Assert

            // Assert

            foreach (var trainingEvent in nextup)
            {
                Console.WriteLine(trainingEvent.DayName);

                foreach (var participant in trainingEvent.Participants)
                    Console.WriteLine(participant.Name + " " + participant.Avatar );

            }
        }

    }
}
