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

namespace Auction.Controllers.EntitiesControllers
{
    public class LotsController : Controller
    {
        private ApplicationDbContext db;

        public LotsController( ApplicationDbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            db = context;
        }

        // GET: Lots
        public ActionResult Index()
        {

            var lotsWithStakes = ApplicationDbContext.GetLotsAndStakesViewModel();
            return View(lotsWithStakes);
        }

        // GET: Lots/Details/5
        public ActionResult Details(int? id)
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

        // GET: Lots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LotId,Name,Description,ImagePath,HoursDuration,InitialStake")] Lot lot)
        {
            if (ModelState.IsValid)
            {
                db.Lots.Add(lot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lot);
        }

        // GET: Lots/Edit/5
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

        // POST: Lots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LotId,Name,Description,ImagePath,HoursDuration,InitialStake")] Lot lot)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lot);
        }

        // GET: Lots/Delete/5
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

        // POST: Lots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lot lot = db.Lots.Find(id);
            db.Lots.Remove(lot);
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
