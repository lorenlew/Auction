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
        public string Name { get; set; }

        [Display(Name = "Lot description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [Display(Name = "Duration (hours)")]
        public int HoursDuration { get; set; }

        [Display(Name = "Initial stake")]
        public int InitialStake { get; set; }

        [Display(Name = "Last date of stake")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastDateOfStake { get; set; }

        [Display(Name = "Stake timeout")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StakeTimeout { get; set; }

        [Display(Name = "Last stake")]
        public int? LastStake { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name = "Is available")]
        public bool IsAvailable { get; set; }

        public bool IsSold { get; set; }
    }
}