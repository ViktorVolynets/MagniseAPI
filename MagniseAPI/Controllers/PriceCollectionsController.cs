using AutoMapper;
using MagniseAPI.Helpers;
using MagniseAPI.Models;
using MagniseAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace MagniseAPI.Controllers
{
    [ApiController]
    [Route("api/pricecollections")]
    public class PriceCollectionsController : Controller
    {
        private readonly IPricesRepository _pricesRepository;
        private readonly IMapper _mapper;

        public PriceCollectionsController(
            IPricesRepository pricesRepository, 
            IMapper mapper)
        {
            _pricesRepository = pricesRepository ??
                throw new ArgumentNullException(nameof(pricesRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        // api/pricecollections?{assetIds}
        [HttpGet]
        public async Task<IActionResult> GetPriceCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
             [FromQuery] IEnumerable<Guid> assetIds)
        {
            var priceEntities = await _pricesRepository.GetPricesAsync(assetIds);

            if (assetIds.Count() != priceEntities.Select(x=>x.AssetId).Distinct().Count())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<PriceDto>>(priceEntities));
        }
    }
}
