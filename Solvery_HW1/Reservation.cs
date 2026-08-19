using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvery_HW1
{
    class Reservation
    {
        public int ReservationId {  get; set; }
        public string ClientName { get; set; }
        public int HotelId {  get; set; }

        public Reservation(int reservationId, string clientName, int hotelId) 
        {
            ReservationId = reservationId;
            ClientName = clientName;
            HotelId = hotelId;
        }
    }
}
