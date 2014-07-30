using System.Net;
using System.Web.Mvc;
using Auction.Interfaces;
using Auction.Repositories;
using Auction.Web.ViewModels;

namespace Auction.Web.Controllers
{
    public class StakesController : Controller
    {
        private readonly IUnitOfWork _uow;

        public StakesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize]
        public ActionResult Create(int? id, double? stakeIncrease)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stakeIncrease = stakeIncrease ?? 1.05;
            var currentLot = ViewModelsLogic.GetCurrentLot((int)id, _uow);
            if (currentLot == null)
            {
                return HttpNotFound();
            }
            if (!currentLot.IsAvailable)
            {
                return View("LotIsSold");
            }
            var currentStake = ViewModelsLogic.GetCurrentStake((int)id, stakeIncrease, currentLot);
            if (currentStake == null)
            {
                return HttpNotFound();
            }
            _uow.StakeRepository.Create(currentStake);
            _uow.Save();
            return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
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
