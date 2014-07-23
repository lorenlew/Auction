using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.Models.DomainModels
{
    public class Lot
    {
        public int LotId { get; set; }

        [Display(Name = "Lot name")]
        [Required(ErrorMessage = "Enter lot name")]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Lot description")]
        [Required(ErrorMessage = "Enter lot description")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Display(Name = "Duration (hours)")]
        [Required(ErrorMessage = "Enter duration of lot in auction")]
        public int HoursDuration { get; set; }

        [Display(Name = "Initial stake")]
        [Required(ErrorMessage = "Enter initial stake")]
        public int InitialStake { get; set; }

        public virtual ICollection<Stake> Stakes { get; set; }

    }
}