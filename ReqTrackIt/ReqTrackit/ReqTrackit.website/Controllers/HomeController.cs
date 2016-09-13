using System.Web.Mvc;
using ReqTrackit.website.Models.ReqTrackModel;

namespace ReqTrackit.website.Controllers
{
    public class HomeController : Controller
    {
        private ReqTrackEnities _db=new ReqTrackEnities();

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}