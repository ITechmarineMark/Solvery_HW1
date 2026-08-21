using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Data-класс Отель, инстанцируется данными
/// из файла dz_1.txt
/// </summary>
namespace Solvery_HW1
{
    class Hotel
    {
        public int HotelId {  get; set; }
        public string HotelName { get; set; }
        public string HotelType { get; set; }
        public string HotelAddress { get; set; }
        public int Capacity { get; set; }

        public Hotel() { }

        public Hotel(int hotelId, string hotelName, string hotelType, string hotelAddress, int capacity)
        {
            HotelId = hotelId;
            HotelName = hotelName;
            HotelType = hotelType;
            HotelAddress = hotelAddress;
            Capacity = capacity;
        }
    }
}
