using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Auction.Models;
using Auction.Models.ViewModels;
using WebGrease.Css.Extensions;

namespace Auction.Controllers
{
    public class UserManipulationController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> UserManager;

        public UserManipulationController(ApplicationDbContext context)
        {
            db = context;
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult UserManagement()
        {
            var users = db.Users;
            ViewBag.userManager = UserManager;
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
            var targetUserId = GetUserId(name, UserManager);
            var isTargetUserModerator = UserManager.IsInRole(targetUserId, "Moderator");

            if (isTargetUserModerator)
            {
                UserManager.RemoveFromRole(targetUserId, "Moderator");
            }
            else
            {
                UserManager.AddToRole(targetUserId, "Moderator");
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
            var targetUser = UserManager.FindByName(name);
            if (targetUser == null)
            {
                return UserManagement();
            }
            var isUserPerformingActionModerator = UserManager.IsInRole(User.Identity.GetUserId(), "Moderator");
            var isTargetUserModerator = UserManager.IsInRole(targetUser.Id, "Moderator");

            if (User.IsInRole("Administrator") || (isUserPerformingActionModerator && !isTargetUserModerator))
            {
                targetUser.IsBanned = !targetUser.IsBanned;
                UserManager.Update(targetUser);
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