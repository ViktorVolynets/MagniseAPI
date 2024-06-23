using MagniseAPI.DbContexts;
using MagniseAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace MagniseAPI.Services
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly MarketContext _context;

        public AssetsRepository(MarketContext context) 
        { 
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all assets synchronously.
        /// </summary>
        /// <returns>The collection of assets.</returns>
        public IEnumerable<Asset> GetAssets()
        {
            return _context.Assets.ToList();
        }

        /// <summary>
        /// Retrieves a list of all assets asynchronously.
        /// </summary>
        /// <returns>The collection of assets.</returns>
        public async Task<IEnumerable<Asset>> GetAssetsAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        /// <summary>
        /// Adds a new asset to the database.
        /// </summary>
        /// <param name="asset">The asset to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the asset parameter is null.</exception>

        public void AddAsset(Asset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            _context.Assets.Add(asset);
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>True if any changes were saved successfully; otherwise, false.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
