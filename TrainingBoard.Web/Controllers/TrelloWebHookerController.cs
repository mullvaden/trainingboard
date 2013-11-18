using System.Web.Mvc;

namespace TrainingBoard.Web.Controllers
{
    public class TrelloWebHookerController : Controller
    {
        public ActionResult Index()
        {
            return Json("OK");
        }

        [HttpPost]
        public ActionResult TakeAction()
        {
            return Json("OK");
        }
    }
}