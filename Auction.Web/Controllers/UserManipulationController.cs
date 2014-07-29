using System.Net;
using System.Web.Mvc;
using Auction.DAL;
using Auction.Domain.Models;
using Auction.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Web.Controllers
{
    public class UserManipulationController : Controller
    {
        private readonly UnitOfWork _uow;

        public UserManipulationController( UnitOfWork uow)
        {
            _uow = uow;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult UserManagement()
        {
            var users = _uow.UserRepository.Read();
            ViewBag.userManager = _uow.UserManager;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserManipulation", users);
            }
            return View(users);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult SetToRole(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var targetUserId = GetUserId(name);
            if (targetUserId == null)
            {
                return HttpNotFound();
            }
            var isTargetUserModerator = _uow.UserManager.IsInRole(targetUserId, "Moderator");
            if (isTargetUserModerator)
            {
                _uow.UserManager.RemoveFromRole(targetUserId, "Moderator");
            }
            else
            {
                _uow.UserManager.AddToRole(targetUserId, "Moderator");
            }
            return UserManagement();
        }

        private string GetUserId(string name)
        {
            var targetUser = _uow.UserManager.FindByName(name);
            var targetUserId = targetUser.Id;
            return targetUserId;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult ChangeUserAccess(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var targetUser = _uow.UserManager.FindByName(name);
            if (targetUser == null)
            {
                return HttpNotFound();
            }
            var isUserPerformingActionModerator = _uow.UserManager.IsInRole(User.Identity.GetUserId(), "Moderator");
            var isTargetUserModerator = _uow.UserManager.IsInRole(targetUser.Id, "Moderator");

            if (User.IsInRole("Administrator") || (isUserPerformingActionModerator && !isTargetUserModerator))
            {
                targetUser.IsBanned = !targetUser.IsBanned;
                _uow.UserManager.Update(targetUser);
            }
            return UserManagement();
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