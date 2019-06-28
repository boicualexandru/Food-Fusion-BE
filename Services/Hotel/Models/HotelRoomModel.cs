using System.Collections.Generic;

namespace Services.Hotel.Models
{
    public class HotelRoomModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int Floor { get; set; }
        
        public int Beds { get; set; }
        
        public int MaxGuests { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public IList<HotelFeatureModel> Features { get; set; }
    }
}
