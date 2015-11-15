using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.DAL;
using ARideShare.Models;

namespace ARideShare.BL
{
    public class RideCordinatesBL
    {
        RideCordinatesRepository waypoints_ = new RideCordinatesRepository();

        public List<RideCordinate> GetRideCordinatesByRideId(int id)
        {
            return waypoints_.GetRideCordinatesById(id);
        }
        public bool AddWaypoints(RideCordinate waypoints)
        {
            return waypoints_.AddWaypoints(waypoints);
        }
        public bool Delete(int ride_id)
        {
           return  waypoints_.Delete(ride_id);
        }
        public bool Edit(RideCordinate waypoint)
        {
            return waypoints_.Edit(waypoint);
        }
    }
}