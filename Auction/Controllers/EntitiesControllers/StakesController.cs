using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auction.Models;
using Auction.Models.DomainModels;
using Auction.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Controllers.EntitiesControllers
{
    public class StakesController : Controller
    {
        private ApplicationDbContext db;

        public StakesController(ApplicationDbContext context)
        {
            db = context;
        }

        [Authorize]
        public ActionResult Create(int id, double? stakeIncrease)
        {
            stakeIncrease = stakeIncrease ?? 1.05;
            var currentLot = ApplicationDbContext.GetCurrentLot(id);
            if (!currentLot.IsAvailable)
            {
                return View("LotIsSold");
            }
            var currentStake = ApplicationDbContext.GetCurrentStake(id, stakeIncrease, currentLot);
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
