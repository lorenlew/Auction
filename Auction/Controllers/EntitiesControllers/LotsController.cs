using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
            db = context;
        }

        // GET: Lots
        public ActionResult Index(bool? isAjax)
        {
            bool isAjaxRequest = isAjax.GetValueOrDefault();
            var lotsWithStakes = ApplicationDbContext.GetLotsAndStakesViewModel();
            if (isAjaxRequest)
            {
                return PartialView("_lotsAndStakes", lotsWithStakes);
            }
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
        public ActionResult Create([Bind(Include = "LotId,Name,Description,Image,HoursDuration,InitialStake")] Lot lot)
        {
            if (ModelState.IsValid)
            {
                string fileName = Guid.NewGuid() + "." + Path.GetExtension((lot.Image.FileName)).Substring(1);
                string virtualPath = "/Content/Images/LotImages/" + fileName;
                string physicalPath = HttpContext.Server.MapPath(virtualPath);

                lot.Image.SaveAs(physicalPath);
                lot.ImagePath = virtualPath;
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
        public ActionResult Edit([Bind(Include = "LotId,Name,Description,Image,HoursDuration,InitialStake")] Lot lot)
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
            string physicalPath = HttpContext.Server.MapPath(lot.ImagePath);
            System.IO.File.Delete(physicalPath);
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
