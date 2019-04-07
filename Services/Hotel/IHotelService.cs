﻿using DataAccess.Models;
using Services.Hotel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Hotel
{
    public interface IHotelService
    {
        IList<HotelRoomModel> GetRooms(HotelRoomsFiltersModel filters = null);

        IList<HotelFeatureModel> GetAvailableFeatures();
    }
}
