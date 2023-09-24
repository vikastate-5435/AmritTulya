using AmritTulya.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmritTulya.Repository
{
    public class UserMasterRepository : IDisposable
    {
        AmritTulyaDbContext context = new AmritTulyaDbContext();
        public UsersEntity ValidateUser(string username, string password)
        {
            UsersEntity usersEntity = new UsersEntity();
         if(username== "admin@gmail.com" && password == "12345")
            {
                usersEntity = GetDummyUser();
            }
            else
            {
                usersEntity= context.UsersEntities.FirstOrDefault(user =>
             user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
             && user.Password == password);
            }
            return usersEntity;
        }
        public void Dispose()
        {
            context.Dispose();
        }

        public UsersEntity GetDummyUser()
        {
            UsersEntity user = new UsersEntity();
            user.Id = 1000000;
            user.Username = "admin@gmail.com";
            user.Password = "12345";
            user.userRole = "admin";
            user.CreatedDate = DateTime.Now.AddDays(-10);
            user.LastLoginDate = DateTime.Now;
            return user;
        }
    }
}
