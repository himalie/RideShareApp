using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.DAL;
using ARideShare.Models;

namespace ARideShare.BL
{
    public class RideBL
    {
        RideRepository ride_repository = new RideRepository();

        public List<Ride> GetAllRides()
        {
            return ride_repository.GetAllRides();
        }
        public bool AddRide(Ride ride)
        {
            return ride_repository.AddRide(ride);
        }
        public Ride GetRideById(int id)
        {
            return ride_repository.GetRideById(id);
        }
        public List<Ride> GetRidesByUser(int id)
        {
            return ride_repository.GetRidesByUser(id);
        }
        public List<Ride> GetRideByLocations(String from_, String to_)
        {
            return ride_repository.GetRideByLocations(from_, to_);
        }
        public bool Delete(int ride_id)
        {
            return ride_repository.Delete(ride_id);
        }
        public bool Edit(Ride ride)
        {
            return ride_repository.Edit(ride);
        }
    }
}