using System.ComponentModel.DataAnnotations;

namespace Auction.Domain.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
