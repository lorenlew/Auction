using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Auction.Domain.DerivativeModels;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Auction.Web.ViewModels;
using AutoMapper;

namespace Auction.Web.Controllers
{
    public class LotsController : Controller
    {
        private readonly ILotService _lotService;
        private readonly IStakeService _stakeService;
        private readonly IUserManagerService _userManagerService;

        public LotsController(ILotService lotService, IStakeService stakeService, IUserManagerService userManagerService)
        {
            if (lotService == null) throw new ArgumentNullException("lotService");
            if (stakeService == null) throw new ArgumentNullException("stakeService");
            if (userManagerService == null) throw new ArgumentNullException("userManagerService");
            _lotService = lotService;
            _stakeService = stakeService;
            _userManagerService = userManagerService;
        }

        public ActionResult Index(bool? isAjax)
        {
            bool isAjaxRequest = isAjax ?? false;
            var lotsWithStakes = _lotService.GetAvailable();
            var lotStakeViewModel = Mapper.Map<IEnumerable<LotStakeDomainModel>, IEnumerable<LotStakeViewModel>>(lotsWithStakes);
            if (isAjaxRequest)
            {
                return PartialView("_lotsAndStakes", lotStakeViewModel);
            }
            return View(lotStakeViewModel);
        }


        [Authorize(Roles = "Administrator, Moderator")]
        public async Task<ActionResult> SoldLots()
        {
            await CheckWonStakesAsync();
            var soldLots = _lotService.GetSold();
            var lotStakeViewModel = Mapper.Map<IEnumerable<LotStakeDomainModel>, IEnumerable<LotStakeViewModel>>(soldLots);
            return View(lotStakeViewModel);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Image,HoursDuration,InitialStake")] LotViewModel lot)
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

            var lotDomain = Mapper.Map<LotViewModel, LotDomainModel>(lot);
            _lotService.Add(lotDomain);
            _lotService.Save();
            lot.Image.SaveAs(physicalPath);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LotDomainModel lot = _lotService.ReadById((int) id);
            if (lot == null)
            {
                return HttpNotFound();
            }
            var lotViewModel = Mapper.Map<LotDomainModel, LotViewModel>(lot);
            return View(lotViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ImagePath,HoursDuration,InitialStake")] LotViewModel lot)
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
            var lotDomain = Mapper.Map<LotViewModel, LotDomainModel>(lot);
            _lotService.Update(lotDomain);
            _lotService.DisableValidationOnSave();
            _lotService.Save();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult DeleteConfirmed(int id, bool? isMain)
        {
            bool isMainPage = isMain ?? false;
            LotDomainModel lot = _lotService.ReadById(id);
            string physicalPath = HttpContext.Server.MapPath(lot.ImagePath);
            System.IO.File.Delete(physicalPath);
            _lotService.Delete(lot);
            _lotService.Save();
            if (isMainPage)
            {
                var lotStakeViewModel = Mapper.Map<IEnumerable<LotStakeDomainModel>, IEnumerable<LotStakeViewModel>>(_lotService.GetAvailable());
                return PartialView("_lotsAndStakes", lotStakeViewModel);
            }
            if (Request.IsAjaxRequest())
            {
                var lotStakeViewModel = Mapper.Map<IEnumerable<LotStakeDomainModel>, IEnumerable<LotStakeViewModel>>(_lotService.GetSold());
                return PartialView("_soldLots", lotStakeViewModel);
            }
            return RedirectToAction("Index");
        }

        #region serviceMethods
        private async Task CheckWonStakesAsync()
        {
            var notReportedStakes = (from lots in _lotService.GetAll()
                                     where !lots.IsSold && !lots.IsAvailable
                                     select lots).ToList();

            if (notReportedStakes.Any())
            {
                await SendNotificationsToWinnersAsync(notReportedStakes);
            }
        }

        private async Task SendNotificationsToWinnersAsync(IEnumerable<LotStakeDomainModel> notReportedStakes)
        {
            foreach (var stake in notReportedStakes)
            {
                string emailBody = "<h2>You've won '" + stake.Name +
                                   "'. Win date - " + stake.StakeTimeout + ". Use personal id to get lot.</h2>";

                await _userManagerService.SendEmailAsync(stake.ApplicationUserId, "Attention!", emailBody);
                _lotService.ReadById(stake.LotId).IsSold = true;
                _lotService.DisableValidationOnSave();
                _lotService.Save();
            }
        }

        private bool IsLotNameUsed(LotViewModel lot)
        {
            var lotsWithSameName = from l in _lotService.Read()
                                   where l.Name == lot.Name && lot.Id != l.Id
                                   select l;
            var isNameUsed = lotsWithSameName.Any();
            return isNameUsed;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lotService.Dispose();
                _stakeService.Dispose();
                _userManagerService.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
