using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ARideShare.DAL;
using ARideShare.Models;

namespace ARideShare.BL
{
    public class UserBL
    {
        UserRepository user_repository = new UserRepository();

        public IEnumerable<User> GetAllUsers()
        {
            return user_repository.GetAllUsers();
        }

        public bool RegisterUser(User user)
        {
            return user_repository.SaveUser(user);
        }
        public User GetUserById(int id)
        {
            return user_repository.GetUserById(id);
        }
        public User GetUserByUserName(string user_name)
        {
            return user_repository.GetUserByUserName(user_name);
        }
        public bool LoginValid(string username, string password)
        {
            return user_repository.IsValid(username, password);
        }
        public bool Edit(User edit_user)
        {
            return user_repository.Edit(edit_user);
        }
        public bool Delete(int user_id)
        {
            return user_repository.Delete(user_id);
        }
        public User GetUserByToken(string user_name, string token)
        {
            return user_repository.GetUserByToken(user_name, token);
        }

        //for my password testing !!
        //public string Encode(string value)
        //{
        //    return user_repository.Encode(value);
        //}
    }
}