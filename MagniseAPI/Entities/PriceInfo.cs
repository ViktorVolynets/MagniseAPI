using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagniseAPI.Entities
{
    public class PriceInfo
    {
        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey("AssetId")]
        public Asset Asset { get; set; } = null!;

        public Guid AssetId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime UpdateTime { get; set; }

        public PriceInfo(Guid id, Guid assetId, decimal price, DateTime updateTime)
        {
            Id = id;
            AssetId = assetId;
            Price = price;
            UpdateTime = updateTime;
        }
    }
}
