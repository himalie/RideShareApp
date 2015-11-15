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
    public class RiderInfoController : ApiController
    {
        RiderInfoBL rider = new RiderInfoBL();

        public HttpResponseMessage Get()
        {
            #region variables
            RiderInfo rider_ = null;
            IEnumerable<RiderInfo> riders= null;
            var response = Request.CreateResponse();
            #endregion varables

            var queryValues = Request.RequestUri.ParseQueryString();
            int ride_id_ = 0;
            int user_id_ = 0;
            if (queryValues.Count > 0)
            {
                if (queryValues.HasKeys())
                {
                    if ((object)queryValues["ride_id"] != null)
                        ride_id_ = Int32.Parse(queryValues["ride_id"]);
                    if ((object)queryValues["user_id"] != null)
                        user_id_ = Int32.Parse(queryValues["user_id"]);
                }
            }
            if ((ride_id_ != 0) && (user_id_ != 0))
            {
                // fetch record when ride id and user id is given
                rider_ = rider.GetRiderofRide(ride_id_, user_id_);
                response = Request.CreateResponse(HttpStatusCode.OK, rider_);
            }
            else if ((ride_id_ != 0) && (user_id_ == 0))
            {
                // rider by ride id
                riders = rider.GetRidersByRideId(ride_id_);
                response = Request.CreateResponse(HttpStatusCode.OK, riders);
            }
            else if ((user_id_ != 0) && (ride_id_ == 0))
            {
                // rider by user id
                riders = rider.GetRidesByUser(user_id_);
                response = Request.CreateResponse(HttpStatusCode.OK, riders);
            }
            

            HttpContext context = HttpContext.Current;
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Post(RiderInfo rider)
        {
            // if state is not valid  method aborts the request and returns a Bad Request (400) status code
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            this.rider.SaveRider(rider);

            HttpContext context = HttpContext.Current;

            var response = Request.CreateResponse(HttpStatusCode.Created, rider);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;

        }

        [HttpPut]
        public HttpResponseMessage SaveRider(RiderInfo rider_)
        {
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            if (rider_ == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            bool result = rider.Edit(rider_);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        // DELETE api/user/5
        [HttpDelete]
        public HttpResponseMessage DeleteRider(int ride_id, int user_id)
        {
            var user = rider.GetRiderofRide(ride_id, user_id);
            if (user == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));
            bool result = rider.Delete(ride_id, user_id);
            
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
