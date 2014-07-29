using System.Data.Entity.Migrations;
using Auction.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.DAL.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Auction.DAL.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Administrator"))
            {
                var adminRole = roleManager.Create(new IdentityRole("Administrator"));
            }
            if (!roleManager.RoleExists("Moderator"))
                roleManager.Create(new IdentityRole("Moderator"));


            if (userManager.FindByName("admin@admin.net") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@admin.net", 
                    Email = "admin@admin.net", 
                    EmailConfirmed = true, 
                };
                userManager.Create(user, "1q2w3eQ`");

                userManager.AddToRole(user.Id, "Administrator");
            }
        }
    }
}
