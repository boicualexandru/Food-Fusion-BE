using Common;
using System.Collections.Generic;

namespace Services.Hotel.Models
{
    public class HotelRoomsFiltersModel
    {
        public int Guests { get; set; }

        public IList<int> FeatureIds { get; set; }

        public TimeRange TimeRange { get; set; }
    }
}
