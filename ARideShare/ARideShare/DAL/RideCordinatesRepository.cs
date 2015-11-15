using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.Models;
using System.Data;

namespace ARideShare.DAL
{
    public class RideCordinatesRepository
    {
        private const string CacheKey = "RideWaypointsStore";
        Entities db = new Entities();

        public RideCordinatesRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var ride_cordinates = db.RideCordinates.ToList();
                    ctx.Cache[CacheKey] = ride_cordinates;
                }
            }
        }

        public List<RideCordinate> GetRideCordinatesById(int id)
        {
            //var ctx = HttpContext.Current;

            //var currentData = ((List<RideCordinate>)ctx.Cache[CacheKey]).ToList();
            //List<RideCordinate> ride_cordinates = currentData.Where(p => p.ride_id == id).ToList();
            //if (ride_cordinates != null)
            //{
            //    // if cache is not null return from cache
            //    return ride_cordinates;
            //}

            // if not data in cache return from database
            List<RideCordinate> aa = db.RideCordinates.Where(p => p.ride_id == id).ToList();
            if (aa.Any())
                return aa.ToList();
            else
                return null;
        }

        public bool AddWaypoints(RideCordinate waypoints)
        {
            try
            {
                db.RideCordinates.Add(waypoints);
                db.SaveChanges();

               // return db.RideCordinates.Last().ride_id;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //return 0;
                return false;
            }
        }

        public bool Edit(RideCordinate waypoint)
        {
            try
            {
                var ctx = HttpContext.Current;
                try
                {
                    if (ctx != null)
                    {
                        var currentData = ((List<RideCordinate>)ctx.Cache[CacheKey]).ToList();
                        RideCordinate waypoint_ = currentData.Find(p => p.ride_id == waypoint.ride_id );
                        currentData.Remove(waypoint_);
                        currentData.Add(waypoint);
                        ctx.Cache[CacheKey] = currentData.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }

                db.Entry(waypoint).State = EntityState.Modified;
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
                List<RideCordinate> ways = db.RideCordinates.Where(p => p.ride_id == ride_id).ToList();
                foreach (var waypoint_ in ways)
                {
                    db.RideCordinates.Remove(waypoint_);
                    db.SaveChanges();
                }

                
                return true;

            }
            catch (Exception ex)
            {
                // Shud do some logging
                Console.WriteLine(ex.ToString());
                //throw new System.Data.DataException();
                return false;
            }
        }

    }
}