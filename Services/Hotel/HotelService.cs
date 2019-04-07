using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
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
                    hotelRooms = hotelRooms.Where(r => r.Reservations
                        .All(res => res.EndTime <= filters.TimeRange.Start || res.StartTime >= filters.TimeRange.End));
                }
            }

            var hotelRoomsList = hotelRooms.ToList();

            return _mapper.Map<IList<HotelRoomModel>>(hotelRoomsList);
        }
    }
}
