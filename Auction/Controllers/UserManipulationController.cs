using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Auction.DAL;
using Auction.DAL.DomainModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebGrease.Css.Extensions;

namespace Auction.Controllers
{
    public class UserManipulationController : Controller
    {
        private ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManipulationController(ApplicationDbContext context)
        {
            db = context;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult UserManagement()
        {
            var users = db.Users;
            ViewBag.userManager = _userManager;
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
                return UserManagement(); ;
            }
            var targetUserId = GetUserId(name, _userManager);
            var isTargetUserModerator = _userManager.IsInRole(targetUserId, "Moderator");
            if (isTargetUserModerator)
            {
                _userManager.RemoveFromRole(targetUserId, "Moderator");
            }
            else
            {
                _userManager.AddToRole(targetUserId, "Moderator");
            }
            return UserManagement();
        }

        private string GetUserId(string name, UserManager<ApplicationUser> userManager)
        {
            var targetUser = userManager.FindByName(name);
            var targetUserId = targetUser.Id;
            return targetUserId;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult ChangeUserAccess(string name)
        {
            if (name == null)
            {
                return UserManagement();
            }
            var targetUser = _userManager.FindByName(name);
            if (targetUser == null)
            {
                return UserManagement();
            }
            var isUserPerformingActionModerator = _userManager.IsInRole(User.Identity.GetUserId(), "Moderator");
            var isTargetUserModerator = _userManager.IsInRole(targetUser.Id, "Moderator");

            if (User.IsInRole("Administrator") || (isUserPerformingActionModerator && !isTargetUserModerator))
            {
                targetUser.IsBanned = !targetUser.IsBanned;
                _userManager.Update(targetUser);
            }
            return UserManagement();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}