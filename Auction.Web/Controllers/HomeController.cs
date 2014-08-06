using System;
using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Auction.Web.ViewModels;
using AutoMapper;

namespace Auction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        public HomeController( IUserManagerService userManagerService)
        {
            if (userManagerService == null) throw new ArgumentNullException("userManagerService");
            _userManagerService = userManagerService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            var currentUser = _userManagerService.FindByName(User.Identity.Name);
            var applicationUserViewModel = Mapper.Map<ApplicationUserDomainModel, ApplicationUserViewModel>(currentUser);
            return View(applicationUserViewModel);
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