using AutoMapper;
using DataAccess.Models;
using Services.Cuisines.Models;

namespace Services.Cuisines.Mappers
{
    public class CuisineProfile : Profile
    {
        public CuisineProfile()
        {
            CreateMap<Cuisine, CuisineModel>();
            CreateMap<CuisineModel, Cuisine>()
                .ForMember(cuisine => cuisine.Id, opt => opt.Ignore());
        }
    }
}
