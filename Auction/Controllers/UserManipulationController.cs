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
        public async Task<ActionResult> UserManagement()
        {
            var users = db.Users;
            ViewBag.userManager = UserManager;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserManipulation", users);
            }
            await CheckWonStakesAsync();
            return View(users);
        }

        private async Task CheckWonStakesAsync()
        {
            var notReportedStakes = (from lots in ApplicationDbContext.GetLotsAndStakesViewModel()
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

        [Authorize(Roles = "Administrator")]
        public Task<ActionResult> SetToRole(string name)
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
        public Task<ActionResult> ChangeUserAccess(string name)
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