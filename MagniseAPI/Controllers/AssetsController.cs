using AutoMapper;
using MagniseAPI.Models;
using MagniseAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MagniseAPI.Controllers
{
    [ApiController]
    [Route("api/assets")]
    public class AssetsController : Controller
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly IMapper _mapper;
        public AssetsController(IAssetsRepository assetsRepository, 
            IMapper mapper)
        {
            _assetsRepository = assetsRepository ??
                throw new ArgumentNullException(nameof(assetsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetAssets")]
        public async Task<IActionResult> GetAssets()
        {
            var assets = await _assetsRepository
                .GetAssetsAsync();

            return Ok(_mapper.Map<IEnumerable<AssetDto>>(assets));
        }
    }
}
