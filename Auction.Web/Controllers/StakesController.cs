using System;
using System.Net;
using System.Web.Mvc;
using Auction.Services.Interfaces;

namespace Auction.Web.Controllers
{
    public class StakesController : Controller
    {
        private readonly ILotService _lotService;
        private readonly IStakeService _stakeService;

        public StakesController(ILotService lotService, IStakeService stakeService)
        {
            if (lotService == null) throw new ArgumentNullException("lotService");
            if (stakeService == null) throw new ArgumentNullException("stakeService");
            _lotService = lotService;
            _stakeService = stakeService;
        }

        [Authorize]
        public ActionResult Create(int? id, double? stakeIncrease)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stakeIncrease = stakeIncrease ?? _stakeService.MinStakeRate;
            var currentLot = _lotService.FindById((int)id);
            if (currentLot == null)
            {
                return HttpNotFound();
            }
            if (!currentLot.IsAvailable)
            {
                return View("LotIsSold");
            }
            var currentStake = _stakeService.Create((int)id, stakeIncrease, currentLot);
            if (currentStake == null)
            {
                return HttpNotFound();
            }
            _stakeService.Add(currentStake);
            _stakeService.Save();
            return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lotService.Dispose();
                _stakeService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
