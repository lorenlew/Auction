using Auction.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Auction.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Auction.Models.ApplicationDbContext";
        }

        protected override void Seed(Auction.Models.ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists("Administrator"))
            {
                var adminRole = roleManager.Create(new IdentityRole("Administrator"));
            }
            if (!roleManager.RoleExists("Moderator"))
                roleManager.Create(new IdentityRole("Moderator"));


            if (userManager.FindByName("admin@admin.net") == null)
            {
                var user = new ApplicationUser { UserName = "admin@admin.net", Email = "admin@admin.net", EmailConfirmed = true };
                userManager.Create(user, "1q2w3eQ`");

                userManager.AddToRole(user.Id, "Administrator");
            }
        }
    }
}
