namespace MagniseAPI.Models
{
    public class PriceDto
    {
        public Guid AssetId { get; set; }

        public decimal Price { get; set; }

        public DateTime UpdateTime { get; set; }

        public PriceDto(Guid assetId, decimal price, DateTime updateTime)
        {
            AssetId = assetId;
            Price = price;
            UpdateTime = updateTime;
        }
    }
}
