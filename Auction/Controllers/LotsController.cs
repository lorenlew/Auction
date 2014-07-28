using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Auction.DAL;
using Auction.DAL.DomainModels;
using Auction.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace Auction.Controllers
{
    public class LotsController : Controller
    {
        private ApplicationDbContext db;

        public LotsController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult Index(bool? isAjax)
        {
            bool isAjaxRequest = isAjax ?? false;
            var lotsWithStakes = ViewModelsLogic.GetAvailableLotsAndStakesViewModel(db);
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
            var soldLots = ViewModelsLogic.GetSoldLotsAndStakesViewModel(db);
            return View(soldLots);
        }

        private async Task CheckWonStakesAsync()
        {
            var notReportedStakes = (from lots in ViewModelsLogic.GetLotsAndStakesViewModel(db)
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
                    db.Lots.Find(stake.LotId).IsSold = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
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
        public ActionResult Create([Bind(Include = "LotId,Name,Description,Image,HoursDuration,InitialStake")] Lot lot)
        {
            if (lot == null)
            {
                throw new ArgumentNullException("lot");
            }
            if (!ModelState.IsValid)
            {
                return View(lot);
            }
            if (!IsLotNameUnique(lot))
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

            db.Lots.Add(lot);
            db.SaveChanges();
            lot.Image.SaveAs(physicalPath);
            return RedirectToAction("Index");
        }

        private bool IsLotNameUnique(Lot lot)
        {
            var currentLot = from l in db.Lots
                          where l.Name == lot.Name
                          select l;
            var isNameUnique = !currentLot.Any();
            return isNameUnique;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lot lot = db.Lots.Find(id);
            if (lot == null)
            {
                return HttpNotFound();
            }
            return View(lot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Edit([Bind(Include = "LotId,Name,Description,ImagePath,HoursDuration,InitialStake")] Lot lot)
        {
            if (lot == null)
            {
                throw new ArgumentNullException("lot");
            }
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(lot);
            }
            db.Entry(lot).State = EntityState.Modified;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lot lot = db.Lots.Find(id);
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
            Lot lot = db.Lots.Find(id);
            string physicalPath = HttpContext.Server.MapPath(lot.ImagePath);
            System.IO.File.Delete(physicalPath);
            db.Lots.Remove(lot);
            db.SaveChanges();
            if (isMainPage)
            {
                return PartialView("_lotsAndStakes", ViewModelsLogic.GetAvailableLotsAndStakesViewModel(db));
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_soldLots", ViewModelsLogic.GetSoldLotsAndStakesViewModel(db));
            }
            return RedirectToAction("Index");
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
