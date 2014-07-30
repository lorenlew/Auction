using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Auction.Domain.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Auction.Web.ViewModels
{
    public class ApplicationUserViewModel : IdentityUser
    {
        [StringLength(30)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [StringLength(30)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public bool IsBanned { get; set; }

        public virtual ICollection<Stake> Stakes { get; set; }

    }
}