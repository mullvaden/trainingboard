using System.Web.Mvc;

namespace TrainingBoard.Web.Controllers
{
    public class TrelloWebHookerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TakeAction()
        {
            return Json("OK");
        }
    }
}