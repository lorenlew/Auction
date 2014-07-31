using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Auction.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Ninject.Infrastructure.Language;

namespace Auction.Web.Controllers
{
    public class UserManipulationController : Controller
    {
        private readonly IUserManagerService _userManagerService;

        public UserManipulationController(IUserManagerService userManagerService)
        {
            _userManagerService = userManagerService;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult UserManagement()
        {
            var users = _userManagerService.GetAccess().Users.ToEnumerable();
            ViewBag.userManager = _userManagerService.GetAccess();
            var usersViewModel = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(users);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_UserManipulation", usersViewModel);
            }
            return View(usersViewModel);
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
            var isTargetUserModerator = _userManagerService.GetAccess().IsInRole(targetUserId, "Moderator");
            if (isTargetUserModerator)
            {
                _userManagerService.GetAccess().RemoveFromRole(targetUserId, "Moderator");
            }
            else
            {
                _userManagerService.GetAccess().AddToRole(targetUserId, "Moderator");
            }
            return UserManagement();
        }

        private string GetUserId(string name)
        {
            var targetUser = _userManagerService.GetAccess().FindByName(name);
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
            var targetUser = _userManagerService.GetAccess().FindByName(name);
            if (targetUser == null)
            {
                return HttpNotFound();
            }
            var isUserPerformingActionModerator = _userManagerService.GetAccess().IsInRole(User.Identity.GetUserId(), "Moderator");
            var isTargetUserModerator = _userManagerService.GetAccess().IsInRole(targetUser.Id, "Moderator");

            if (User.IsInRole("Administrator") || (isUserPerformingActionModerator && !isTargetUserModerator))
            {
                targetUser.IsBanned = !targetUser.IsBanned;
                _userManagerService.GetAccess().Update(targetUser);
            }
            return UserManagement();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManagerService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}