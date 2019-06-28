using AutoMapper;
using Common;
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


            CreateMap<HotelRoomReservation, HotelReservationDetailedModel>()
                .ForMember(rd => rd.Range, opt => opt
                    .MapFrom(r => new TimeRange
                    {
                        Start = r.StartTime,
                        End = r.EndTime
                    }));
            CreateMap<HotelReservationDetailedModel, HotelRoomReservation>()
                .ForMember(reservation => reservation.Id, opt => opt.Ignore());
        }
    }
}
