using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Auction.Domain.Attributes;

namespace Auction.Domain.Models
{
    public class Lot : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public HttpPostedFileBase Image { get; set; }

        public string ImagePath { get; set; }

        public int HoursDuration { get; set; }

        public int InitialStake { get; set; }

        public bool IsSold { get; set; }
        public virtual ICollection<Stake> Stakes { get; set; }
    }
}