using System;
using System.Net;
using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Interfaces;
using Auction.Web.ViewModels;
using Microsoft.AspNet.Identity;

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
            var currentLot = LotStakeViewModel.GetCurrentLot((int)id, _uow);
            if (currentLot == null)
            {
                return HttpNotFound();
            }
            if (!currentLot.IsAvailable)
            {
                return View("LotIsSold");
            }
            var currentStake = GetCurrentStake((int)id, stakeIncrease, currentLot);
            if (currentStake == null)
            {
                return HttpNotFound();
            }
            _uow.StakeRepository.Create(currentStake);
            _uow.Save();
            return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
        }

        private Stake GetCurrentStake(int id, double? stakeIncrease, LotStakeViewModel currentLot)
        {
            if (currentLot == null) throw new ArgumentNullException("currentLot");
            var currentStake = new Stake
            {
                LotId = id,
                ApplicationUserId = User.Identity.GetUserId(),
                DateOfStake = DateTime.Now
            };

            if (currentLot.LastStake == null)
            {
                currentStake.StakeTimeout = DateTime.Now.AddHours(currentLot.HoursDuration);
                currentStake.CurrentStake = currentLot.InitialStake;
            }
            else
            {
                currentStake.StakeTimeout = currentLot.StakeTimeout.GetValueOrDefault().AddMinutes(1);
                currentStake.CurrentStake = (int)(currentLot.LastStake * stakeIncrease);
            }
            return currentStake;
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
