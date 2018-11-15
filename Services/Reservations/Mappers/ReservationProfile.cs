using AutoMapper;
using DataAccess.Models;
using Services.Reservations.Models;

namespace Services.Reservations.Mappers
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationModel>();
            CreateMap<ReservationModel, Reservation>()
                .ForMember(reservation => reservation.Id, opt => opt.Ignore());

            CreateMap<Reservation, ReservationDetailedModel>();
            CreateMap<ReservationDetailedModel, Reservation>()
                .ForMember(reservation => reservation.Id, opt => opt.Ignore());
        }
    }
}
