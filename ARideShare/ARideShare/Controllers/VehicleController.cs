using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARideShare.Models;
using ARideShare.BL;
using System.Web;

namespace ARideShare.Controllers
{
    public class VehicleController : ApiController
    {
        private VehicleBL vehicle;
        public VehicleController()
        {
            this.vehicle = new VehicleBL();
        }
        // GET api/vehicle
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var queryValues = Request.RequestUri.ParseQueryString();
            int user = 0;
            int vehicle_id = 0;
            List<Vehicle> vehicles = new List<Vehicle>();
            if (queryValues.Count > 0)
            {
                if (queryValues.HasKeys())
                {
                    Int32.TryParse(queryValues["user"], out user);
                    Int32.TryParse(queryValues["id"], out vehicle_id);
                }
            }
            if ((user != 0) && (vehicle_id == 0))
            {
                vehicles = vehicle.GetVehicleByUser(user);
            }
            else
            {
                Vehicle vehicle_ = vehicle.GetVehicleById(vehicle_id);
                vehicles.Add(vehicle_);
            }
            
            if (vehicles == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            HttpContext context = HttpContext.Current;
            var response = Request.CreateResponse(HttpStatusCode.OK, vehicles);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        // GET api/vehicle/user=1
        // get a list of vehicles owned by a user
        public List<Vehicle> GetVehicleByUser(int id)
        {
            var queryValues = Request.RequestUri.ParseQueryString();
            int user = 0;
            int idd = 0;
            if (queryValues.Count > 0)
            {
                if (queryValues.HasKeys())
                {
                    Int32.TryParse(queryValues["user"], out user);
                    Int32.TryParse(queryValues["id"], out idd);
                }
            }

            var vehicles = vehicle.GetVehicleByUser(user);
            if (vehicles == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            return vehicles.ToList();
        }

        // POST api/vehicle
        [HttpPost]
        public HttpResponseMessage Post(Vehicle vehicle_)
        {
            // if state is not valid  method aborts the request and returns a Bad Request (400) status code
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            this.vehicle.AddVehicle(vehicle_);

            HttpContext context = HttpContext.Current;

            var response = Request.CreateResponse(HttpStatusCode.Created, vehicle_);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }

        // PUT api/vehicle/5
        [HttpPut]
        public HttpResponseMessage Put(Vehicle vehicle_)
        {
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            if (vehicle_ == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            bool result = this.vehicle.Edit(vehicle_);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        // DELETE api/user/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            var user = vehicle.GetVehicleById(id);
            if (user == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));
            bool result = vehicle.Delete(id);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
