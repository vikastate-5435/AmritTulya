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
            return context.UsersEntities.FirstOrDefault(user =>
            user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.Password == password);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
