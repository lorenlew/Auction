﻿using System;

namespace Auction.Domain.Models
{
    public class StakeDomainModel : EntityDomainModel
    {
        public int CurrentStake { get; set; }

        public DateTime DateOfStake { get; set; }

        public DateTime StakeTimeout { get; set; }

        public int LotId { get; set; }

        public string ApplicationUserId { get; set; }
    }
}