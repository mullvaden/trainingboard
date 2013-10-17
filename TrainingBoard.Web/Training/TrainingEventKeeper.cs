using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TrainingBoard.TrelloIntegrator;
using TrainingBoard.Web.Hubs;

namespace TrainingBoard.Web.Training
{
    public class TrainingEventKeeper
    {
        // Singleton instance
        private readonly static Lazy<TrainingEventKeeper> _instance = new Lazy<TrainingEventKeeper>(() => new TrainingEventKeeper(GlobalHost.ConnectionManager.GetHubContext<TrainingHub>().Clients));

        private readonly ConcurrentDictionary<string, TrainingEvent> _trainingEvents = new ConcurrentDictionary<string, TrainingEvent>();

        private readonly object _updateTrainingEventLock = new object();

        //stock can go up or down by a percentage of this factor on each change

        private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(1);
        private readonly Random _updateOrNotRandom = new Random();

        private readonly Timer _timer;
        private volatile bool _updatingTrainingEvents = false;
        private DateTime _fetchedDateTime;
        private IClock _clock;
        private FetchUp _fetchUp;

        private TrainingEventKeeper(IHubConnectionContext clients)
        {
            Clients = clients;

            _trainingEvents.Clear();
            _clock = new Clock();
            _fetchUp = new FetchUp(_clock);
            GetTrainingEvents();

            _fetchedDateTime = _clock.Now;

            _timer = new Timer(UpdateTrainingEvents, null, _updateInterval, _updateInterval);

        }

        private void GetTrainingEvents()
        {
            var nextup = _fetchUp.GetUpcoming(1);
            nextup.ForEach(training => _trainingEvents.TryAdd(training.DayName, training));
            _fetchedDateTime = _clock.Now;
        }

        public static TrainingEventKeeper Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public IEnumerable<TrainingEvent> GetAllTrainingEvents()
        {
            return _trainingEvents.Values.OrderBy(t => t.Day);
        }

        private void UpdateTrainingEvents(object state)
        {
            lock (_updateTrainingEventLock)
            {
                if (!_updatingTrainingEvents)
                {
                    _updatingTrainingEvents = true;
                    _trainingEvents.Clear();
                    //if (_fetchedDateTime.Day != _clock.Now.Day)
                    //{
                        GetTrainingEvents();
                        BroadcastUpdateTrainingEvents();

                    //}
                    _updatingTrainingEvents = false;
                }
            }
        }

        private void BroadcastUpdateTrainingEvents()
        {
            Clients.All.updateTrainingEvents(GetAllTrainingEvents());
        }

    }
}