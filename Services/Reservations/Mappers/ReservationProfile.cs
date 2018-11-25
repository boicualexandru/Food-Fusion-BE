using AutoMapper;
using Common;
using DataAccess.Models;
using Services.Reservations.Models;
using System.Collections.Generic;
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


            CreateMap<ReservationRequestModel, Reservation>()
                .ForMember(reservation => reservation.StartTime, opt => opt.MapFrom(r => r.Range.Start))
                .ForMember(reservation => reservation.EndTime, opt => opt.MapFrom(r => r.Range.End))
                .ForMember(reservation => reservation.ReservedTables, opt => opt.MapFrom(r => r.TableIds
                    .Select(tId => new ReservedTable
                    {
                        RestaurantTableId = tId
                    })));
        }
    }
}
