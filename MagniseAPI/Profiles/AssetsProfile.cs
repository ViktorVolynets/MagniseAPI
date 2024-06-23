using AutoMapper;
using MagniseAPI.Entities;
using MagniseAPI.Models;

namespace MagniseAPI.Profiles;

public class AssetsProfile : Profile
{
	public AssetsProfile()
	{
        CreateMap<Asset, AssetDto>();
        CreateMap<AssetForCreationDto, Asset>();
    }
}
