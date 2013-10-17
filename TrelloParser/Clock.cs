using System;

namespace TrainingBoard.TrelloIntegrator
{
    public class Clock : IClock
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}