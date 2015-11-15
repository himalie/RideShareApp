using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARideShare.Models;
using ARideShare.BL;
using ARideShare.DAL;
using System.Web;

namespace ARideShare.Controllers
{
    public class UserController : ApiController
    {
        private UserBL user_bl;

        public UserController()
        {
            this.user_bl = new UserBL();
        }


        // GET api/user/5
        [HttpGet]
        public HttpResponseMessage Get()
        {
            #region variables
            User user = null;
            Boolean login_status = false;
            #endregion varables

            var queryValues = Request.RequestUri.ParseQueryString();
            string user_name_ = "";
            string password_ = "";
            int user_id_ =  0;
            string token = "";
            if (queryValues.Count > 0)
            {
                if (queryValues.HasKeys())
                {
                    if ((object)queryValues["user_name"] != null)
                        user_name_ = queryValues["user_name"].ToString();
                    if ((object)queryValues["password"] != null)
                        password_ = queryValues["password"].ToString();
                    if ((object)queryValues["user_id"] != null)
                    {
                        if (queryValues["user_id"].ToString() != "")
                            user_id_ = Int32.Parse(queryValues["user_id"]);
                    }
                    if ((object)queryValues["token"] != null)
                    {
                        if (queryValues["token"].ToString() != "")
                            token = queryValues["token"].ToString();
                    }
                }
            }
                
            if (((user_name_ != "") && (password_ != "")) && (user_id_ == 0))
            {
                // if login data are valid
                login_status = user_bl.LoginValid(user_name_, password_);
                if (login_status)
                {
                    // fetch that user record
                    user = user_bl.GetUserByUserName(user_name_);
                }
                else
                {
                    // if not return null
                    user = null;
                }
            }
            else if ((user_name_ != "") && (password_ == "") && (user_id_ == 0) && (token == ""))
            {
                user = user_bl.GetUserByUserName(user_name_);
            }
            else if ((user_id_ != 0) && (user_name_ == "") && (password_ == "") && (token == ""))
            {
                user = user_bl.GetUserById(user_id_);
            }
            else if ((user_name_ != "") && (token != "") && (password_ == "") && (user_id_ == 0))
            {
                // fetch user record by the token saved in the data base when a user logs in to the application
                user = user_bl.GetUserByToken(user_name_, token);
            }

            HttpContext context = HttpContext.Current;
            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            #region variables
            User user = null;
            #endregion varables
            user = user_bl.GetUserById(id);
                        
            HttpContext context = HttpContext.Current;
            var response = Request.CreateResponse(HttpStatusCode.OK, user);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }


        [HttpPost]
        public HttpResponseMessage Post(User user)
        {
            // if state is not valid  method aborts the request and returns a Bad Request (400) status code
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            this.user_bl.RegisterUser(user);

            HttpContext context = HttpContext.Current;

            var response = Request.CreateResponse(HttpStatusCode.Created, user);
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }


        // PUT api/user/5
        [HttpPut]
        public HttpResponseMessage PutUser(User user)
        {
            if (!this.ModelState.IsValid)
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            if (user == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));

            bool result = user_bl.Edit(user);            
;
            var response = Request.CreateResponse(HttpStatusCode.NoContent);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "PUT");
            return response;
        }

        // DELETE api/user/5
        public HttpResponseMessage DeleteUser(int id)
        {
            var user = user_bl.GetUserById(id);
            if (user == null)
                throw new HttpResponseException(this.Request.CreateResponse(HttpStatusCode.NotFound));
            bool result =user_bl.Delete(id);            

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            return response;
        }
    }
}
