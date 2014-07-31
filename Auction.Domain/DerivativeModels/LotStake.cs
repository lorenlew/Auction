using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.Domain.DerivativeModels
{
    public class LotStake
    {
        public int LotId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public int HoursDuration { get; set; }

        public int InitialStake { get; set; }

        public DateTime? LastDateOfStake { get; set; }

        public DateTime? StakeTimeout { get; set; }

        public int? LastStake { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name = "Is available")]
        public bool IsAvailable { get; set; }

        public bool IsSold { get; set; }
    }
}