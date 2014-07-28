using System.Net;
using System.Web.Mvc;
using Auction.DAL;
using Auction.ViewModels;

namespace Auction.Controllers
{
    public class StakesController : Controller
    {
        private ApplicationDbContext db;

        public StakesController(ApplicationDbContext context)
        {
            db = context;
        }

        [Authorize]
        public ActionResult Create(int? id, double? stakeIncrease)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stakeIncrease = stakeIncrease ?? 1.05;
            var currentLot = ViewModelsLogic.GetCurrentLot((int)id, db);
            if (currentLot == null)
            {
                return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
            }
            if (!currentLot.IsAvailable)
            {
                return View("LotIsSold");
            }
            var currentStake = ViewModelsLogic.GetCurrentStake((int)id, stakeIncrease, currentLot);
            if (currentStake == null)
            {
                return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
            }
            db.Stakes.Add(currentStake);
            db.SaveChanges();
            return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
