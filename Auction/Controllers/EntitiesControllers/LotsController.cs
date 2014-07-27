﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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

        public LotsController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult Index(bool? isAjax)
        {
            bool isAjaxRequest = isAjax ?? false;
            var lotsWithStakes = ApplicationDbContext.GetLotsAndStakesViewModel();
            if (isAjaxRequest)
            {
                return PartialView("_lotsAndStakes", lotsWithStakes);
            }
            return View(lotsWithStakes);
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
            if (ModelState.IsValid)
            {
                string fileName = Guid.NewGuid() + "." + Path.GetExtension((lot.Image.FileName)).Substring(1);
                string virtualPath = "/Content/Images/LotImages/" + fileName;
                string physicalPath = HttpContext.Server.MapPath(virtualPath);
                lot.ImagePath = virtualPath;
                try
                {
                    db.Lots.Add(lot);
                    db.SaveChanges();
                    lot.Image.SaveAs(physicalPath);

                }
                catch (DbUpdateException e)
                {
                    SqlException innerException = null;
                    Exception tmp = e;
                    while (innerException == null && tmp != null)
                    {
                        if (tmp != null)
                        {
                            innerException = tmp.InnerException as SqlException;
                            tmp = tmp.InnerException;
                        }

                    }
                    if (innerException != null && innerException.Number == 2601)
                    {
                        ModelState.AddModelError("", "Name " + lot.Name + " is already taken.");
                        return View(lot);
                    }
                    throw;
                }
                return RedirectToAction("Index");
            }
            return View(lot);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Moderator")]
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
