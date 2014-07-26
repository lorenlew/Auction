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

        // GET: Stakes
        public ActionResult Index()
        {
            return View(db.Stakes.ToList());
        }

        // GET: Stakes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stake stake = db.Stakes.Find(id);
            if (stake == null)
            {
                return HttpNotFound();
            }
            return View(stake);
        }

        // GET: Stakes/Create
        public ActionResult Create(int id, double? stakeIncrease)
        {
            stakeIncrease = stakeIncrease ?? 1.05;

            var currentLot = (from lots in ApplicationDbContext.GetLotsAndStakesViewModel()
                              where lots.LotId == id
                              select lots).Single();

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
            db.Stakes.Add(currentStake);
            db.SaveChanges();
            return RedirectToAction("Index", "Lots", new { isAjax = Request.IsAjaxRequest() });
        }


        // GET: Stakes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stake stake = db.Stakes.Find(id);
            if (stake == null)
            {
                return HttpNotFound();
            }
            return View(stake);
        }

        // POST: Stakes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StakeId,HoursForAuctionEnd,CurrentStake,DateOfStake,LotId,ApplicationUserId")] Stake stake)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stake).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stake);
        }

        // GET: Stakes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stake stake = db.Stakes.Find(id);
            if (stake == null)
            {
                return HttpNotFound();
            }
            return View(stake);
        }

        // POST: Stakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stake stake = db.Stakes.Find(id);
            db.Stakes.Remove(stake);
            db.SaveChanges();
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
