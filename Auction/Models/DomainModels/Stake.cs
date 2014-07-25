using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Auction.Models.DomainModels
{
    public class Stake
    {
        public int StakeId { get; set; }

        [Display(Name = "Time for auction's end")]
        public int HoursForAuctionEnd { get; set; }

        [Display(Name = "Current stake")]
        public int CurrentStake { get; set; }

        [Display(Name = "Date of stake")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfStake { get; set; }

        [Display(Name = "Lot name")]
        [Required]
        public int LotId { get; set; }

        [Display(Name = "User name")]
        [Required]
        public string ApplicationUserId { get; set; }
    }
}