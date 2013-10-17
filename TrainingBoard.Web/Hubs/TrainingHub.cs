using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using TrainingBoard.TrelloIntegrator;
using TrainingBoard.Web.Training;

namespace TrainingBoard.Web.Hubs
{
    public class TrainingHub : Hub
    {
        private readonly TrainingEventKeeper _trainingKeeper;

        public TrainingHub() : this(TrainingEventKeeper.Instance) { }

        public TrainingHub(TrainingEventKeeper trainingEventKeeper)
        {
            _trainingKeeper = trainingEventKeeper;
        } 

        public IEnumerable<TrainingEvent> GetAllTrainingEvents()
        {
            return _trainingKeeper.GetAllTrainingEvents();
        }
    }
}