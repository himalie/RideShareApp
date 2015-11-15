using ARideShare.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ARideShare.DAL
{
    public class VehicleRepository
    {
        private const string CacheKey = "VehicleStore";
        Entities db = new Entities();

        public VehicleRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var vehicles = db.Vehicles.ToList();
                    ctx.Cache[CacheKey] = vehicles;
                }
            }
        }

        // get all vehicles
        public List<Vehicle> GetAllVehicles()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                // if cache is not null return from cache
                return (List<Vehicle>)ctx.Cache[CacheKey];
            }

            // if not data in cache return from database
            return db.Vehicles.ToList();
        }

        // get vehicle by id
        public Vehicle GetVehicleById(int id)
        {
            //var ctx = HttpContext.Current;
            //var currentData = ((List<Vehicle>)ctx.Cache[CacheKey]).ToList();
            //if (ctx != null)
            //{
            //    // if cache is not null return from cache
            //    return (User)ctx.Cache[CacheKey];
            //}

            // if not data in cache return from database
            return db.Vehicles.Find(id);
        }

        // get vehicle by vehicle number which is unique
        public String GetVehicleNo(string no)
        {
            var ctx = HttpContext.Current;
            var currentData = ((List<Vehicle>)ctx.Cache[CacheKey]).ToList();
            //if (ctx != null)
            //{
            //    // if cache is not null return from cache
            //    return (User)ctx.Cache[CacheKey];
            //}

            // if not data in cache return from database
            String vehicle_no = (from vehicle in  db.Vehicles
                            where (vehicle.vehicle_no == no)
                            select vehicle.vehicle_no).First();

            return vehicle_no;
        }

        // get vehicles of a  user
        public List<Vehicle> GetVehicleByUser(int user_id)
        {
            //var ctx = HttpContext.Current;
            //var currentData = ((List<Vehicle>)ctx.Cache[CacheKey]).ToList();
            //if (ctx != null)
            //{
            //    // if cache is not null return from cache
            //    return (User)ctx.Cache[CacheKey];
            //}

            // if not data in cache return from database
            List<Vehicle> vehicles = db.Vehicles.Where(p => p.user_id == user_id).ToList();
            return vehicles;
        }

        // insert a vehicle
        public bool AddVehicle(Vehicle vehicle)
        {
            var ctx = HttpContext.Current;
            try
            {
                // add information to the cache
                //var currentData = ((Vehicle)ctx.Cache[CacheKey]);
                //currentData.Add(vehicle);
                //ctx.Cache[CacheKey] = currentData.ToArray();

                // add data to the database
                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool Edit(Vehicle vehicle)
        {
            try
            {
                db.Entry(vehicle).State = EntityState.Modified;
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

        public bool Delete(int vehicle_id)
        {
            try
            {
                Vehicle vehicle = db.Vehicles.Find(vehicle_id);
                db.Vehicles.Remove(vehicle);
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
    }
}