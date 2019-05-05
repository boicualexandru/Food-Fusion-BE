using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Common;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Services.Hotel.Exceptions;
using Services.Hotel.Models;

namespace Services.Hotel
{
    public class HotelService : IHotelService
    {
        private readonly FoodFusionContext _dbContext;
        private readonly IMapper _mapper;

        public HotelService(FoodFusionContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<HotelFeatureModel> GetAvailableFeatures()
        {
            var features = _dbContext.HotelFeatures.ToList();

            return _mapper.Map<IList<HotelFeatureModel>>(features);
        }

        public IList<HotelRoomModel> GetRooms(HotelRoomsFiltersModel filters = null)
        {
            var hotelRooms = _dbContext.HotelRooms
                .Include(r => r.RoomFeatures)
                    .ThenInclude(rf => rf.Feature)
                .Include(r => r.Reservations)
                .AsQueryable();

            if(filters != null)
            {
                if(filters.Guests > 0)
                {
                    hotelRooms = hotelRooms.Where(r => r.MaxGuests >= filters.Guests);
                }

                filters.FeatureIds = filters.FeatureIds ?? new List<int>();
                foreach (var featureId in filters.FeatureIds)
                {
                    hotelRooms = hotelRooms.Where(r => r.RoomFeatures.Any(rf => rf.FeatureId == featureId));
                }

                if(filters.TimeRange != null)
                {
                    // correction for dates containing time
                    filters.TimeRange.Start = filters.TimeRange.Start.Date;
                    filters.TimeRange.End = filters.TimeRange.End.Date;

                    hotelRooms = hotelRooms.Where(r => r.Reservations
                        .All(res => res.EndTime <= filters.TimeRange.Start || res.StartTime >= filters.TimeRange.End));
                }
            }

            var hotelRoomsList = hotelRooms.ToList();

            return _mapper.Map<IList<HotelRoomModel>>(hotelRoomsList);
        }

        public void BookRoom(HotelRoomBookingModel bookingDetails)
        {
            bookingDetails = bookingDetails ?? throw new HotelBookingInvalidDetailesException();

            var room = _dbContext.HotelRooms
                .Include(r => r.Reservations)
                .FirstOrDefault(r => r.Id == bookingDetails.RoomId)
                ?? throw new HotelRoomNotFoundException();
            
            if (bookingDetails.Guests > room.MaxGuests)
            {
                throw new HotelRoomGuestsExceededException();
            }
            
            // correction for dates containing time
            bookingDetails.Range.Start = bookingDetails.Range.Start.Date;
            bookingDetails.Range.End = bookingDetails.Range.End.Date;

            if (!IsRoomAvailable(room, bookingDetails.Range))
            {
                throw new HotelRoomUnavailableException();
            }

            room.Reservations.Add(new HotelRoomReservation
            {
                UserId = bookingDetails.UserId,
                StartTime = bookingDetails.Range.Start,
                EndTime = bookingDetails.Range.End,
                GuestsCount = bookingDetails.Guests
            });
            _dbContext.SaveChanges();
        }

        private bool IsRoomAvailable(HotelRoom room, TimeRange range)
        {
            return room.Reservations.All(res => res.EndTime <= range.Start || res.StartTime >= range.End);
        }

        public IList<HotelReservationDetailedModel> GetReservations(int? userId = null)
        {
            var reservations = _dbContext.HotelRoomReservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .AsQueryable();

            if (userId.HasValue)
            {
                reservations = reservations.Where(r => r.UserId == userId);
            }

            var reservationList = reservations.ToList();
            return _mapper.Map<IList<HotelReservationDetailedModel>>(reservationList);
        }
    }
}
