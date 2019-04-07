using AutoMapper;
using DataAccess.Models;
using Services.Hotel.Models;
using System.Linq;

namespace Services.Hotel.Mappers
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<HotelRoom, HotelRoomModel>()
                .ForMember(hotelRoomModel => hotelRoomModel.Features, 
                    opt => opt.MapFrom(hotelRoom => hotelRoom.RoomFeatures.Select(rf => rf.Feature)));
            CreateMap<HotelRoomModel, HotelRoom>()
                .ForMember(hotelRoom => hotelRoom.Id, opt => opt.Ignore());

            CreateMap<HotelFeature, HotelFeatureModel>();
            CreateMap<HotelFeatureModel, HotelFeature>()
                .ForMember(hotelFeature => hotelFeature.Id, opt => opt.Ignore());
        }
    }
}
