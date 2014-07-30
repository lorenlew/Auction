using System.Linq;
using System.Web.Mvc;
using Auction.Repositories;
using Microsoft.AspNet.Identity;

namespace Auction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _uow;

        public HomeController(UnitOfWork uow)
        {
            _uow = uow;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            var currentUser = _uow.UserManager.FindByName(User.Identity.Name);
            return View(currentUser);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}