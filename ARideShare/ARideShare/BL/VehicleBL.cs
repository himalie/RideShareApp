using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.Models;
using ARideShare.DAL;

namespace ARideShare.BL
{
    public class VehicleBL
    {
        VehicleRepository vehicle_repository = new VehicleRepository();

        public List<Vehicle> GetAllVehicles()
        {
            return vehicle_repository.GetAllVehicles();
        }

        public bool AddVehicle(Vehicle vehicle)
        {
            return vehicle_repository.AddVehicle(vehicle);
        }
        public List<Vehicle> GetVehicleByUser(int id)
        {
            return vehicle_repository.GetVehicleByUser(id);
        }
        public Vehicle GetVehicleById(int id)
        {
            return vehicle_repository.GetVehicleById(id);
        }
        public bool Delete(int vehicle_id)
        {
            return vehicle_repository.Delete(vehicle_id);
        }
        public bool Edit(Vehicle vehicle)
        {
            return vehicle_repository.Edit(vehicle);
        }

    }
}