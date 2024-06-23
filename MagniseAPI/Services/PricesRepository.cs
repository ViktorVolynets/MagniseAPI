using MagniseAPI.DbContexts;
using MagniseAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MagniseAPI.Services
{
    public class PricesRepository : IPricesRepository
    {
        private readonly MarketContext _context;

        public PricesRepository(MarketContext context) 
        { 
            _context = context;
        }

        /// <summary>
        /// Retrieves prices for a specific asset synchronously.
        /// </summary>
        /// <param name="assetId">The ID of the asset for which to retrieve prices.</param>
        /// <returns>A collection of price information for the asset.</returns>
        public IEnumerable<PriceInfo> GetPrices(Guid assetId)
        {
            return _context.Prices.Where(x => x.AssetId.Equals(assetId)).ToList<PriceInfo>();
        }

        /// <summary>
        /// Retrieves prices for a specific asset asynchronously.
        /// </summary>
        /// <param name="assetId">The ID of the asset for which to retrieve prices.</param>
        /// <returns>A collection of price information for the asset.</returns>
        public async Task<IEnumerable<PriceInfo>> GetPricesAsync(Guid assetId)
        {
            return await _context.Prices.Where(x => x.AssetId.Equals(assetId)).ToListAsync<PriceInfo>();
        }

        /// <summary>
        /// Retrieves latest prices for multiple assets asynchronously.
        /// </summary>
        /// <param name="assetIds">The IDs of the assets for which to retrieve latest prices.</param>
        /// <returns>A collection of latest price information for the assets.</returns>
        public async Task<IEnumerable<PriceInfo>> GetPricesAsync(IEnumerable<Guid> assetIds)
        {
            var latestPrices = await _context.Prices
                    .Where(x => assetIds.Contains(x.AssetId))
                    .GroupBy(x => x.AssetId) 
                    .Select(group => group.OrderByDescending(x => x.UpdateTime).FirstOrDefault())
                    .ToListAsync<PriceInfo>();

            return latestPrices;
        }

        /// <summary>
        /// Checks if a price entry exists for a specific asset asynchronously.
        /// </summary>
        /// <param name="assetId">The ID of the asset to check for price existence.</param>
        /// <returns>True if a price exists for the asset; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if assetId is empty.</exception>
        public async Task<bool> PriceExistsAsync(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(assetId));
            }

            return await _context.Prices.AnyAsync(a => a.AssetId == assetId);
        }

        /// <summary>
        /// Adds a new price entry for a specific asset.
        /// </summary>
        /// <param name="assetId">The ID of the asset to which the price belongs.</param>
        /// <param name="price">The price information to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if assetId or price is null.</exception>
        public void AddPrice(Guid assetId, PriceInfo price)
        {
            if (assetId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(assetId));
            }

            if (price == null)
            {
                throw new ArgumentNullException(nameof(price));
            }

            price.AssetId = assetId;
            _context.Prices.Add(price);
        }

        /// <summary>
        /// Saves changes asynchronously to the underlying database.
        /// </summary>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
