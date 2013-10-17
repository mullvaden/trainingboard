using Microsoft.AspNet.SignalR;
using Owin;

namespace TrainingBoard.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HubConfiguration { EnableJSONP = true };

            app.MapSignalR(config);
        }
    }
}