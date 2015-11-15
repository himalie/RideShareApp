using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.Models;
using System.Data;

namespace ARideShare.DAL
{
    public class RideRepository
    {
        private const string CacheKey = "RideStore";
        Entities db = new Entities();

        public RideRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var rides = db.Rides.ToList();
                    ctx.Cache[CacheKey] = rides;
                }
            }
        }

        // fetch all rides
        // rides which are passed according to Date and rides which are completed are ignored
        public List<Ride> GetAllRides()
        {
            //var ctx = HttpContext.Current;

            //if (ctx != null)
            //{
            //    // if cache is not null return from cache
            //    return (List<Ride>)ctx.Cache[CacheKey];
            //}

            // if not data in cache return from database
            var today = DateTime.Today;
            var sql = from rides in db.Rides
                      where (rides.start_date >= today) && (rides.status != "Completed")
                      orderby rides.start_date ascending
                      select rides;
            if (sql.Any())
                return sql.ToList();
            else
                return null;
            //return db.Rides.ToList();
        }

        // get ride by id
        public Ride GetRideById(int id)
        {
            //var ctx = HttpContext.Current;

            //var currentData = ((List<Ride>)ctx.Cache[CacheKey]).ToList();
            //Ride ride = currentData.Find(p => p.ride_id == id);
            //if (ride != null)
            //{
            //    // if cache is not null return from cache
            //    return ride;
            //}

            // if not data in cache return from database
            return db.Rides.Find(id);
        }

        // get rides by user
        // rides will be shown to the user for 2 more days aft they are completed in order to add a rating
        public List<Ride> GetRidesByUser(int id)
        {
            //var ctx = HttpContext.Current;

            //var currentData = ((List<Ride>)ctx.Cache[CacheKey]).ToList();
            //Ride ride = currentData.Find(p => p.user_id == id);
            //if (ride != null)
            //{
            //    // if cache is not null return from cache
            //    return ride;
            //}

            // if not data in cache return from database
            //var sql = db.Rides.Where(p => p.user_id == id);


            var today = DateTime.Today;
            // give a time of 2 days
            DateTime check_ = today.AddDays(2);
            var sql = from rides in db.Rides
                      where (rides.user_id == id) && (rides.start_date >= check_) /*&& (rides.status != "Completed")*/
                      orderby rides.start_date ascending
                      select rides;

            if (sql.Any())
                return sql.ToList();
            else
                return null;           
        }

        public List<Ride> GetRideByLocations(String from_, String to_)
        {
            var ctx = HttpContext.Current;

            var currentData = ((List<Ride>)ctx.Cache[CacheKey]).ToList();
            List<Ride> ride = currentData.Where(p => p.from_location.Contains(from_) &&  p.to_location.Contains(to_)).ToList();
            if (ride != null)
            {
                // if cache is not null return from cache
                return ride;
            }


            // if not data in cache return from database
            var sql = from rides in db.Rides
                      where rides.from_location.Contains(from_) && rides.to_location.Contains(to_)
                      && (rides.status != "Cancelled") && (rides.status != "Completed")
                      select rides;
            if (sql.Any())
                return sql.ToList();
            else
                return null;
            
            
        }

        public bool AddRide(Ride ride)
        {
            try {
                db.Rides.Add(ride);
                db.SaveChanges();

                return true;
                //return db.Rides.Last().ride_id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //return 0;
                return false;
            } 
        }

        public bool Edit(Ride ride)
        {
            try
            {
                //var ctx = HttpContext.Current;
                //try
                //{
                //    if (ctx != null)
                //    {
                //        var currentData = ((List<Ride>)ctx.Cache[CacheKey]).ToList();
                //        Ride ride_ = currentData.Find(p => p.ride_id == ride.ride_id);
                //        currentData.Remove(ride_);
                //        currentData.Add(ride);
                //        ctx.Cache[CacheKey] = currentData.ToArray();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //    return false;
                //}
                Ride v = db.Rides.Find(ride.ride_id);
                db.Entry(v).CurrentValues.SetValues(ride);
                db.SaveChanges();
                return true;

            }
            catch (System.Data.DataException ex)
            {
                // Shud do some logging
                Console.WriteLine(ex.ToString());
                //throw new System.Data.DataException();
                return false;
            }
        }


        public bool Delete(int ride_id)
        {
            try
            {
                Ride ride_ = db.Rides.Find(ride_id);
                // delete waypoints connected to that ride
                for(int i = ride_.RideCordinates.Count - 1; i >= 0; i--) {
                    ride_.RideCordinates.Remove(ride_.RideCordinates.ElementAt(i));
                }

                db.SaveChanges();
                db.Rides.Remove(ride_);
                db.SaveChanges();

                var ctx = HttpContext.Current;
                try
                {
                    if (ctx != null)
                    {
                        var currentData = ((List<Ride>)ctx.Cache[CacheKey]).ToList();
                        currentData.Remove(ride_);
                        ctx.Cache[CacheKey] = currentData.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
                return true;

            }
            catch (System.Data.DataException ex)
            {
                // Shud do some logging
                Console.WriteLine(ex.ToString());
                //throw new System.Data.DataException();
                return false;
            }
        }
    }
}