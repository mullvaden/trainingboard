using System;

namespace TrainingBoard.TrelloIntegrator
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}