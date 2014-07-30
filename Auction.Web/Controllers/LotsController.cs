using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Interfaces;
using Auction.Repositories;
using Auction.Web.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Auction.Web.Controllers
{
    public class LotsController : Controller
    {
        private readonly IUnitOfWork _uow;

        public LotsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ActionResult Index(bool? isAjax)
        {
            bool isAjaxRequest = isAjax ?? false;
            var lotsWithStakes = ViewModelsLogic.GetAvailableLotsAndStakesViewModel(_uow);
            if (isAjaxRequest)
            {
                return PartialView("_lotsAndStakes", lotsWithStakes);
            }
            return View(lotsWithStakes);
        }


        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<ActionResult> SoldLots()
        {
            await CheckWonStakesAsync();
            var soldLots = ViewModelsLogic.GetSoldLotsAndStakesViewModel(_uow);
            return View(soldLots);
        }

        private async Task CheckWonStakesAsync()
        {
            var notReportedStakes = (from lots in ViewModelsLogic.GetLotsAndStakesViewModel(_uow)
                                     where !lots.IsSold && !lots.IsAvailable
                                     select lots).ToList();

            if (notReportedStakes.Any())
            {
                await SendNotificationsToWinnersAsync(notReportedStakes);
            }
        }

        private async Task SendNotificationsToWinnersAsync(IEnumerable<LotViewModel> notReportedStakes)
        {
            var applicationUserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            try
            {
                foreach (var stake in notReportedStakes)
                {
                    string emailBody = "<h2>You've won '" + stake.Name +
                                       "'. Win date - " + stake.StakeTimeout + ". Use personal id to get lot.</h2>";

                    await applicationUserManager.SendEmailAsync(stake.ApplicationUserId, "Attention!", emailBody);
                    _uow.LotRepository.ReadById(stake.LotId).IsSold = true;
                    _uow.DisableValidationOnSave();
                    _uow.Save();
                }
            }
            finally
            {
                applicationUserManager.Dispose();
            }

        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Image,HoursDuration,InitialStake")] Lot lot)
        {
            if (lot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return View(lot);
            }
            if (IsLotNameUsed(lot))
            {
                ModelState.AddModelError("", "Same name is already used");
                return View(lot);
            }
            var extension = Path.GetExtension((lot.Image.FileName));
            if (extension == null)
            {
                return RedirectToAction("Index");
            }

            string fileName = Guid.NewGuid() + "." + extension.Substring(1);
            string virtualPath = "/Content/Images/LotImages/" + fileName;
            string physicalPath = HttpContext.Server.MapPath(virtualPath);
            lot.ImagePath = virtualPath;

            _uow.LotRepository.Create(lot);
            _uow.Save();
            lot.Image.SaveAs(physicalPath);
            return RedirectToAction("Index");
        }

        private bool IsLotNameUsed(Lot lot)
        {
            var lotsWithSameName = from l in _uow.LotRepository.Read()
                             where l.Name == lot.Name && lot.Id != l.Id
                             select l;
            var isNameUsed = lotsWithSameName.Any();
            return isNameUsed;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lot lot = _uow.LotRepository.ReadById(id);
            if (lot == null)
            {
                return HttpNotFound();
            }
            return View(lot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ImagePath,HoursDuration,InitialStake")] Lot lot)
        {
            if (lot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(lot);
            }
            if (IsLotNameUsed(lot))
            {
                ModelState.AddModelError("", "Same name is already used");
                return View(lot);
            }
            _uow.LotRepository.Update(lot);
            _uow.DisableValidationOnSave();
            _uow.Save();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lot lot = _uow.LotRepository.ReadById(id);
            if (lot == null)
            {
                return HttpNotFound();
            }
            return View(lot);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult DeleteConfirmed(int id, bool? isMain)
        {
            bool isMainPage = isMain ?? false;
            Lot lot = _uow.LotRepository.ReadById(id);
            string physicalPath = HttpContext.Server.MapPath(lot.ImagePath);
            System.IO.File.Delete(physicalPath);
            _uow.LotRepository.Delete(lot);
            _uow.Save();
            if (isMainPage)
            {
                return PartialView("_lotsAndStakes", ViewModelsLogic.GetAvailableLotsAndStakesViewModel(_uow));
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_soldLots", ViewModelsLogic.GetSoldLotsAndStakesViewModel(_uow));
            }
            return RedirectToAction("Index");
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
