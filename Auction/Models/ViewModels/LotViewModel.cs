using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Auction.Models.DomainModels;

namespace Auction.Models.ViewModels
{
    public class LotViewModel
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

        [Display(Name = "Last date of stake")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastDateOfStake { get; set; }

        [Display(Name = "Time for auction's end")]
        public int? HoursForAuctionEnd { get; set; }

        [Display(Name = "Last stake")]
        public int? LastStake { get; set; }
    }
}