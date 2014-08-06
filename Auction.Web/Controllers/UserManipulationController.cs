using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Auction.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace Auction.Web.Controllers
{
    public class UserManipulationController : Controller
    {
        private readonly IUserManagerService _userManagerService;

        public UserManipulationController(IUserManagerService userManagerService)
        {
            if (userManagerService == null) throw new ArgumentNullException("userManagerService");
            _userManagerService = userManagerService;
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult UserManagement()
        {
            var users = _userManagerService.GetUsers();
            ViewBag.userManagerService = _userManagerService;
            var usersViewModel = Mapper.Map<IEnumerable<ApplicationUserDomainModel>, IEnumerable<ApplicationUserViewModel>>(users);
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
            var isTargetUserModerator = _userManagerService.IsInRole(targetUserId, "Moderator");
            if (isTargetUserModerator)
            {
                _userManagerService.RemoveFromRole(targetUserId, "Moderator");
            }
            else
            {
                _userManagerService.AddToRole(targetUserId, "Moderator");
            }
            return UserManagement();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult ChangeUserAccess(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var targetUser = _userManagerService.FindByName(name);
            if (targetUser == null)
            {
                return HttpNotFound();
            }
            var isUserPerformingActionModerator = _userManagerService.IsInRole(User.Identity.GetUserId(), "Moderator");
            var isTargetUserModerator = _userManagerService.IsInRole(targetUser.Id, "Moderator");

            if (!User.IsInRole("Administrator") && (!isUserPerformingActionModerator || isTargetUserModerator))
            {
                return UserManagement();
            }
            if (targetUser.IsBanned)
            {
                _userManagerService.UnbanUser(targetUser.Id);
            }
            else
            {
                _userManagerService.BanUser(targetUser.Id);
            }
            return UserManagement();
        }

        #region serviceMethods
        private string GetUserId(string name)
        {
            var targetUser = _userManagerService.FindByName(name);
            var targetUserId = targetUser.Id;
            return targetUserId;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManagerService.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}