using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.Models;
using System.Data;

namespace ARideShare.DAL
{
    public class UserRepository 
    {
        private const string CacheKey = "UserStore";
        Entities db = new Entities();

        // constructor
        public UserRepository()
        {
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                if (ctx.Cache[CacheKey] == null)
                {
                    var users = db.Users.ToList();
                    ctx.Cache[CacheKey] = users;
                }
            }
        }

        // get all users
        public IEnumerable<User> GetAllUsers()
        {
            //HttpContext context = HttpContext.Current;
            //context.Response.AddHeader("Access-Control-Allow-Origin", "*");   
            //var ctx = HttpContext.Current;

            //if (ctx != null)
            //{
            //    // if cache is not null return from cache
            //    return (IEnumerable<User>)ctx.Cache[CacheKey];
            //}

            // if not data in cache return from database
            return db.Users.ToList();
        }

        // get user by id
        public User GetUserById(int id)
        {
            var ctx = HttpContext.Current;

            var currentData = ((List<User>)ctx.Cache[CacheKey]).ToList();
            User user = currentData.Find(p => p.user_id == id);
            if (user != null)
            {
                // if cache is not null return from cache
                return user;
            }

            // if not data in cache return from database
            return db.Users.Find(id);
        }

        // get user by user name which is unique
        public User GetUserByUserName(string user_name)
        {
            var ctx = HttpContext.Current;

            //var currentData = ((List<User>)ctx.Cache[CacheKey]).ToList();
            //User user = currentData.Find(p => p.user_name == user_name);
            //if (user != null)
            //{
            //    // if cache is not null return from cache
            //    return user;
            //}

            // if not data in cache return from database
            List<User> aa = db.Users.Where(p => p.user_name == user_name).ToList();
            if (aa.Any())
                 return aa.First();
            else
                return null;
        }

        // get user by the unique token given when login in to the application
        public User GetUserByToken(string user_name, string token)
        {
            var sql = from user in db.Users
                      where (user.user_name == user_name) && (user.token == token)
                      select user;
            if (sql.Any())
                return sql.First();
            else
                return null;

        }

        // insert a user (register to the application)
        public bool SaveUser(User user)
        {
            var ctx = HttpContext.Current;
            try
            {
                // add information to the cache
                //var currentData = ((List<User>)ctx.Cache[CacheKey]).ToList();
                //currentData.Add(user);
                //ctx.Cache[CacheKey] = currentData.ToArray();

                // add data to the database
                db.Users.Add(user);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }            
        }

        // check whether user login is valid
        public bool IsValid(string username, string password)
        {
            //string passwrd = Encode(password);
            var sql = from users in db.Users
                      where users.user_name == username && users.password == password
                      select users.first_name;
            if (sql.Any())
                return true;
            else
                return false;
        }
        // encode the password
        private string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            return BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
        }

        // edit user details
        public bool Edit(User user)
        {
            try
            {
                //var ctx = HttpContext.Current;
                //try
                //{
                //    if (ctx != null)
                //    {
                //        var currentData = ((List<User>)ctx.Cache[CacheKey]).ToList();
                //        User user_ = currentData.Find(p => p.user_id == user.user_id);
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

                User v = db.Users.Find(user.user_id);
                db.Entry(v).CurrentValues.SetValues(user);

                db.SaveChanges();
                return true;
                
            }
            catch (System.Data.DataException ex)
            {
                // Shud do some logging
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        
        // delete a user 
        public bool Delete(int user_id)
        {
            try
            {                
                User user = db.Users.Find(user_id);
                db.Users.Remove(user);
                db.SaveChanges();

                var ctx = HttpContext.Current;
                try
                {
                    if (ctx != null)
                    {
                        var currentData = ((List<User>)ctx.Cache[CacheKey]).ToList();
                        currentData.Remove(user);
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

        #region IDisposable Members
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}