using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.Models;
using System.Data;

namespace ARideShare.DAL
{
    public class RideInfoRepository
    {
        private const string CacheKey = "RiderInfoStore";
        Entities db = new Entities();

        public RideInfoRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var ride_info = db.RiderInfoes.ToList();
                    ctx.Cache[CacheKey] = ride_info;
                }
            }
        }

        public IEnumerable<RiderInfo> GetRidersByRideId(int id)
        {
            var ctx = HttpContext.Current;
            var currentData = ((List<RiderInfo>)ctx.Cache[CacheKey]).ToList();
            List<RiderInfo> riders = currentData.Where(p => p.ride_id == id).ToList();
            //if (riders != null)
            //{
            //    // if cache is not null return from cache
            //    return riders;
            //}

            // if not data in cache return from database
            return db.RiderInfoes.Where(p => p.ride_id == id).ToList();
        }

        public RiderInfo GetRiderofRide(int ride_id_, int user_id_)
        {
            //var ctx = HttpContext.Current;
            //var currentData = ((List<RiderInfo>)ctx.Cache[CacheKey]).ToList();
            //RiderInfo riders = currentData.Find(ride_id_, user_id_);
            //if (riders != null)
            //{
            //    // if cache is not null return from cache
            //    return riders;
            //}

            // if not data in cache return from database
            return db.RiderInfoes.Find(ride_id_, user_id_);
        }

        // get rides by passenger id
        // time of 2 days given for the users to add ratings
        public IEnumerable<RiderInfo> GetRidesByUser(int id)
        {
            //var ctx = HttpContext.Current;
            //var currentData = ((List<RiderInfo>)ctx.Cache[CacheKey]).ToList();
            //List<RiderInfo> riders = currentData.Where(p => p.user_id == id).ToList();
            //if (riders != null)
            //{
            //    // if cache is not null return from cache
            //    return riders;
            //}

            // if not data in cache return from database
            //return db.RiderInfoes.Where(p => p.user_id == id).ToList();


            var today = DateTime.Today;
            DateTime check_ = today.AddDays(2);
            var sql = from rider in db.RiderInfoes
                      where (rider.user_id == id) && (rider.Ride.start_date >= check_) /*&& (rider.Ride.status != "Completed")*/
                      orderby rider.Ride.start_date ascending
                      select rider;

            if (sql.Any())
                return sql.ToList();
            else
                return null;  
        }

        public bool SaveRider(RiderInfo rider)
        {
            var ctx = HttpContext.Current;
            try
            {
                // add information to the cache
                //var currentData = ((List<RiderInfo>)ctx.Cache[CacheKey]).ToList();
                //currentData.Add(rider);
                //ctx.Cache[CacheKey] = currentData.ToArray();

                // add data to the database
                db.RiderInfoes.Add(rider);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool Edit(RiderInfo rider)
        {
            try
            {
                var ctx = HttpContext.Current;
                //try
                //{
                //    if (ctx != null)
                //    {
                //        var currentData = ((List<RiderInfo>)ctx.Cache[CacheKey]).ToList();
                //        RiderInfo user_ = currentData.Where(p => p.ride_id = rider.ride_id, p => p.user_id = rider.user_id); // Find(p => p.user_id == user.user_id);
                //        currentData.Remove(user_);
                //        currentData.Add(user);
                //        ctx.Cache[CacheKey] = currentData.ToArray();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //    return false;
                //}

                db.Entry(rider).State = EntityState.Modified;
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

        public bool Delete(int ride_id, int user_id)
        {
            try
            {
                RiderInfo rider = (from riders in db.RiderInfoes
                                   where riders.ride_id == ride_id && riders.user_id == user_id
                                   select riders).First();


                db.RiderInfoes.Remove(rider);
                db.SaveChanges();

                var ctx = HttpContext.Current;
                //try
                //{
                //    if (ctx != null)
                //    {
                //        var currentData = ((ListRiderInfoUser>)ctx.Cache[CacheKey]).ToList();
                //        currentData.Remove(user);
                //        ctx.Cache[CacheKey] = currentData.ToArray();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.ToString());
                //    return false;
                //}
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