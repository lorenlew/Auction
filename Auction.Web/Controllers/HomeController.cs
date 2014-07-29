using System.Web.Mvc;
using Auction.DAL;
using Auction.DAL.DomainModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;

        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindByName(User.Identity.Name);
            return View(currentUser);
        }

    }
}