using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Auction.Domain.Models;
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