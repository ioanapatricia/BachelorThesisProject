using AutoMapper;
using SiteManagement.API.Entities;
using SiteManagement.Contracts.Dtos;

namespace SiteManagement.API.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<SiteSettingsForUpdateDto, SiteSettings>().ReverseMap();
            CreateMap<SiteSettings, SiteSettingsForReturnDto>().ReverseMap();
        }
    }
}
