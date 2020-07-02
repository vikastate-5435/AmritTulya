using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmritTulya.EntityLayer
{

    public class AmritTulyaDbContext : DbContext
    {
        public AmritTulyaDbContext()
            : base("AmritTulyaDbContext")
        {
        }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<UsersEntity> UsersEntities { get; set; }
    }
}
