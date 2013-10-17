using System;
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin.Hosting;

namespace TrainingBoard.SignalR
{

    internal class Program
    {
        private static void Main(string[] args)
        {
            string url = "http://localhost:8080/";
            using (WebApplication.Start<Startup>(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }

        public class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.MapHubs(new HubConfiguration { EnableCrossDomain = true });
            }

        }

        public class ChatHub : Hub
        {
            public void Send(string message)
            {
                Clients.All.send(message);
            }
        }
    }
}
