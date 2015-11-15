using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.Models;
using ARideShare.DAL;

namespace ARideShare.BL
{
    public class RiderInfoBL
    {
        RideInfoRepository rider_repository = new RideInfoRepository();

        public IEnumerable<RiderInfo> GetRidersByRideId(int id)
        {
            return rider_repository.GetRidersByRideId(id);
        }
        public IEnumerable<RiderInfo> GetRidesByUser(int id)
        {
            return rider_repository.GetRidesByUser(id);
        }
        public bool SaveRider(RiderInfo rider)
        {
            return rider_repository.SaveRider(rider);
        }
        public bool Edit(RiderInfo rider)
        {
            return rider_repository.Edit(rider);
        }
        public bool Delete(int ride_id, int user_id)
        {
            return rider_repository.Delete(ride_id, user_id);
        }
        public RiderInfo GetRiderofRide(int ride_id_, int user_id_)
        {
            return rider_repository.GetRiderofRide(ride_id_, user_id_);
        }
    }
}