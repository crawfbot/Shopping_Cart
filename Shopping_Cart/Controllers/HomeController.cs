using System.Linq;
using System.Web.Mvc;
using Shopping_Cart.Models;
using Shopping_Cart.Helpers;

namespace Shopping_Cart.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [Authorize(Roles = "Admin")]

        public ActionResult Admin()
      
        {
            ViewBag.Message = "";

            return View();
        }
        public ActionResult Checkout()
        {
            ViewBag.Message = "Checkout";

            return View();
        }        

        public ActionResult Contact()
        {
            ViewBag.Message = "Our Contact Information.";

            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Please Log In.";

            return View();
        }
        public ActionResult Products()
        {
            var itemsList = db.Items.ToList();
            ViewBag.Message = "Products";

            return View(itemsList);
        }
        public ActionResult BedSpreads()
        {
            ViewBag.Message = "Products";

            return View();
        }
        public ActionResult Cushions()
        {
            ViewBag.Message = "Products";

            return View();
        }
        public ActionResult Register()
        {
            ViewBag.Message = "Please Register";

            return View();
        }
        public ActionResult Single()
        {
            ViewBag.Message = "Checkout";

            return View();
        }
    }
}