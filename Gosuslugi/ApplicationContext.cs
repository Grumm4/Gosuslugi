using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gosuslugi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=rootpwd;database=GosuslugiDb;",
                new MySqlServerVersion(new Version(8, 3, 0)));
        }
    }
}
