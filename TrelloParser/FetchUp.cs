using System;
using System.Collections.Generic;
using System.Linq;
using Chello.Core;

namespace TrainingBoard.TrelloIntegrator
{
    public class FetchUp
    {
        private readonly IClock _clock;

        private const string Monday = "525bcdf08e8ccd036e00762e";
        private const string Tuesday = "525bcdf08e8ccd036e00762f";
        private const string Wednesday = "525bcdf08e8ccd036e007630";
        private const string Thursday = "525bce0c4347d2f0480053f5";
        private const string Friday = "525bce11733d249a35006d1f";


        //private const string Monday = "525e6a9a59cd705c56001cc6";
        //private const string Tuesday = "525e6a9a59cd705c56001cc7";
        //private const string Wednesday = "525e6a9a59cd705c56001cc8";
        //private const string Thursday = "525e6b23f3e63dcb76001ef6";
        //private const string Friday = "525e6b288582375e77001898";

        private readonly Dictionary<DayOfWeek, string> _trellolist = new Dictionary<DayOfWeek, string>();

        private readonly ChelloClient _chello;

        private const string AvatarBaseUrl = "https://trello-avatars.s3.amazonaws.com/{0}/50.png";

        public FetchUp(IClock clock)
        {
            _clock = clock;

            FillDayDictionaries();
            _chello = new ChelloClient("73c8985f750f24ec4755c9982c36bf9c", "c415040a8e5a11fbada9e77210242f043c09fc40424725547c54bc8e68240789");
        }

        public List<TrainingEvent> GetUpcoming(int numberOfDaysToLookAhead = 3)
        {
            var upcomingDate = OffSetDateToNextWorkWeekDay(_clock.Now);
            var trainingEvents = new List<TrainingEvent>();

            trainingEvents.Add(GeTrainingEvent(upcomingDate));
            var nextupcomingDate = upcomingDate;
            for (int i = 0; i < numberOfDaysToLookAhead; i++)
            {
                nextupcomingDate = OffSetDateToNextWorkWeekDay(nextupcomingDate.AddDays(1));
                trainingEvents.Add(GeTrainingEvent(nextupcomingDate));

            }

            return trainingEvents;
        }

        private TrainingEvent GeTrainingEvent(DateTime trainingDay)
        {
            var traingEvent = new TrainingEvent();
            var dayName = _chello.Lists.Single(_trellolist[trainingDay.DayOfWeek]).Name;
            var cards = _chello.Cards.ForList(_trellolist[trainingDay.DayOfWeek]);
            var participants = new List<Participant>();

            foreach (var card in cards)
            {
                var participant = new Participant { Name = card.Name };

                var member = _chello.Members.ForCard(card.Id).FirstOrDefault();
                if (member != null)
                {

                    var grav = _chello.Members.AvatarHash(member.Id);
                    if (!string.IsNullOrEmpty(grav) && grav.Length > 25)
                    {
                        grav = grav.Substring(11, grav.Length - 3 - 11);
                        participant.Avatar = string.Format(AvatarBaseUrl, grav);
                    }
                }

                participants.Add(participant);
            }
            traingEvent.Day = trainingDay;
            traingEvent.DayName = dayName;
            traingEvent.Participants = participants;
            return traingEvent;
        }


        private void FillDayDictionaries()
        {
            _trellolist.Add(DayOfWeek.Monday, Monday);
            _trellolist.Add(DayOfWeek.Tuesday, Tuesday);
            _trellolist.Add(DayOfWeek.Wednesday, Wednesday);
            _trellolist.Add(DayOfWeek.Thursday, Thursday);
            _trellolist.Add(DayOfWeek.Friday, Friday);
        }

        private DateTime OffSetDateToNextWorkWeekDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(2);
            if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);
            return date;
        }
    }

    public class TrainingEvent
    {
        public DateTime Day { get; set; }
        public string DayName { get; set; }
        public List<Participant> Participants { get; set; }
    }

    public class Participant
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }
}
