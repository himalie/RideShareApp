using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARideShare.BL;
using ARideShare.Models;
using System.Web;

namespace ARideShare.Controllers
{
    public class RideController : ApiController
    {
        private RideBL ride = new RideBL();

        public RideController()
        {
            //this.ride = new RideBL();
        }
        // GET api/user/5
        public HttpResponseMessage Get()
        {
            #region variables
            List<Ride> rides_ = null;            
            var response = Request.CreateResponse();
            #endregion varables
            
            var queryValues = Request.RequestUri.ParseQueryString();
            int user_id = 0;
            string from_location = "";
            string to_location = "";
            if (queryValues.Count > 0)
            {
                if (queryValues.HasKeys())
                {
                    if ((object)queryValues["user_id"] != null)
                        user_id = Int32.Parse(queryValues["user_id"].ToString());
                    if ((object)queryValues["from_location"] != null)
                        from_location = queryValues["from_location"].ToString();
                    if ((object)queryValues["to_location"] != null)
                        to_location = queryValues["to_location"].ToString();
                }
            }
            HttpContext context = HttpContext.Current;
            if ((user_id != 0) && (from_location == "") && (to_location == ""))
            {
                // fetch rides by user id
                rides_ = ride.GetRidesByUser(user_id);
            }
            else if ((from_location != "") && (to_location != "") && (user_id == 0))
            {
                // fetch rides by locations
                rides_ = ride.GetRideByLocations(from_location, to_location);
            }
            else if ((user_id == 0) && (from_location == "") && (to_location == ""))
            {
                // get all rides
                rides_ = ride.GetAllRides();
            }

            response = Request.CreateResponse(HttpStatusCode.OK, rides_);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        // GET api/user/5
        public HttpResponseMessage GetById(int id)
        {
            #region variables
            Ride ride_ = null;
            var response = Request.CreateResponse();
            #endregion varables
            
            HttpContext context = HttpContext.Current;
            ride_ = ride.GetRideById(id);
 
            response = Request.CreateResponse(HttpStatusCode.OK, ride_);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }


        // add a ride
        [HttpPost]
        public HttpResponseMessage Post(Ride ride_)
        {
            // if state is not valid  method aborts the request and returns a Bad Request (400) status code
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            bool r = this.ride.AddRide(ride_);

            HttpContext context = HttpContext.Current;

            var response = Request.CreateResponse(HttpStatusCode.Created, ride_);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }




        [HttpPut]
        public HttpResponseMessage PutRide(Ride ride_)
        {
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            if (ride_ == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            bool result = ride.Edit(ride_);
            
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "PUT");
            return response;
        }

        // DELETE api/user/5
        [HttpDelete]
        public HttpResponseMessage DeleteRide(int id)
        {
            var user = ride.GetRideById(id);
            if (user == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));
            bool result = ride.Delete(id);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
