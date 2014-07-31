using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Services.Interfaces;
using Auction.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace Auction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IUserManagerService userManagerService)
        {
            _userManager = userManagerService.GetAccess();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            var currentUser = _userManager.FindByName(User.Identity.Name);
            var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(currentUser);
            return View(applicationUserViewModel);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userManager.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}