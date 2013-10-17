using System.Web.Mvc;

namespace TrainingBoard.Web.Controllers
{
    public class TrelloWebHookerController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }
    }
}