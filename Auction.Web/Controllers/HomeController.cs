using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Auction.Domain.Models;
using Auction.Interfaces;
using Auction.Repositories;
using Auction.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace Auction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _uow;

        public HomeController(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserInfo()
        {
            var currentUser = _uow.UserManager.FindByName(User.Identity.Name);
            var applicationUserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(currentUser);
            return View(applicationUserViewModel);
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