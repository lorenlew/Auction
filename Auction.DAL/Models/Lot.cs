using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Auction.DAL.Models
{
    public class Lot : Entity
    {
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        [Required]
        public string Description { get; set; }

        [NotMapped]
        public HttpPostedFileBase Image { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public int HoursDuration { get; set; }

        public int InitialStake { get; set; }

        public bool IsSold { get; set; }

        public virtual ICollection<Stake> Stakes { get; set; }
    }
}