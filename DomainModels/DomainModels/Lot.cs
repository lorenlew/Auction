using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Auction.DAL.Attributes;

namespace Auction.DAL.DomainModels
{
    public class Lot
    {
        public int LotId { get; set; }

        [Display(Name = "Lot name")]
        [Required(ErrorMessage = "Enter lot name")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        [Display(Name = "Lot description")]
        [Required(ErrorMessage = "Enter lot description")]
        [StringLength(500)]
        public string Description { get; set; }

        [NotMapped]
        [FileSize(15360000)]
        [FileTypes("jpg,jpeg,png")]
        [Required(ErrorMessage = "Upload image")]
        public HttpPostedFileBase Image { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [Display(Name = "Duration (hours)")]
        [Required(ErrorMessage = "Enter duration of lot in auction")]
        public int HoursDuration { get; set; }

        [Display(Name = "Initial stake")]
        [Required(ErrorMessage = "Enter initial stake")]
        public int InitialStake { get; set; }

        public bool IsSold { get; set; }
        public virtual ICollection<Stake> Stakes { get; set; }
    }
}