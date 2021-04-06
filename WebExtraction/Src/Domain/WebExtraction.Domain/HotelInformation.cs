using System.Collections.Generic;

namespace WebExtraction.Domain
{
    public class HotelInformation
    {
        public string HotelName { get; set; } = "";
        public string Address { get; set; } = "";
        public int Star { get; set; }
        public HotelPoint Point { get; set; }
        public int NumberOfReviews { get; set; }
        public string Description { get; set; } = "";
        public List<string> RoomCategories{ get; set; } = new List<string>();
        public List<AlternativeHotel> AlternativeHotels{ get; set; } = new List<AlternativeHotel>();

       
    }
}
