using AutoMapper;
using Common;
using DataAccess.Models;
using Services.Reservations.Models;
using System.Linq;

namespace Services.Reservations.Mappers
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<Reservation, ReservationModel>()
                .ForMember(rd => rd.Range, opt => opt
                    .MapFrom(r => new TimeRange
                    {
                        Start = r.StartTime,
                        End = r.EndTime
                    }));
            CreateMap<ReservationModel, Reservation>()
                .ForMember(reservation => reservation.Id, opt => opt.Ignore());

            CreateMap<Reservation, ReservationDetailedModel>()
                .ForMember(rd => rd.Tables, opt => opt
                    .MapFrom(r => r.ReservedTables.Select(rt => rt.Table)))
                .ForMember(rd => rd.Range, opt => opt
                    .MapFrom(r => new TimeRange
                    {
                        Start = r.StartTime,
                        End = r.EndTime
                    }));
            CreateMap<ReservationDetailedModel, Reservation>()
                .ForMember(reservation => reservation.Id, opt => opt.Ignore());
        }
    }
}
