using AutoMapper;

namespace MagniseAPI.Profiles
{
    public class PriceProfiles : Profile
    {
        public PriceProfiles() 
        {
            CreateMap<Entities.PriceInfo, Models.PriceDto>();
        }
    }
}
