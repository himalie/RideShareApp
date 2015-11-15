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
    public class RideCordinatesController : ApiController
    {

        private RideCordinatesBL waypoints;

        public RideCordinatesController()
        {
            this.waypoints = new RideCordinatesBL();
        }


        public HttpResponseMessage Get(int id)
        {
            #region variables
            List<RideCordinate> waypoints_ = null;                       
            #endregion varables
                
            waypoints_ = waypoints.GetRideCordinatesByRideId(id);

            var response = Request.CreateResponse(HttpStatusCode.OK, waypoints_);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Post(RideCordinate waypoints_)
        {
            // if state is not valid  method aborts the request and returns a Bad Request (400) status code
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            bool r = this.waypoints.AddWaypoints(waypoints_);

            var response = Request.CreateResponse(HttpStatusCode.Created, waypoints_);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;


        }

        // PUT api/ridecordinates/5
        public HttpResponseMessage Put(RideCordinate waypoints__)
        {
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            if (waypoints__ == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            bool result = this.waypoints.Edit(waypoints__);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        // DELETE api/user/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            
            bool result = waypoints.Delete(id);

            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
