using System;
using System.ComponentModel.DataAnnotations;

namespace Auction.Domain.Models
{
    public class Stake : Entity
    {
        [Display(Name = "Current stake")]
        public int CurrentStake { get; set; }

        [Display(Name = "Date of stake")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfStake { get; set; }

        [Display(Name = "Stake timeout")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StakeTimeout { get; set; }

        [Display(Name = "Lot name")]
        [Required]
        public int LotId { get; set; }

        [Display(Name = "User name")]
        [Required]
        public string ApplicationUserId { get; set; }
    }
}