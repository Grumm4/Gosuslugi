using Microsoft.EntityFrameworkCore;

namespace Gosuslugi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=grumm;password=password;database=gosuslugidb;",
                new MySqlServerVersion(new Version(8, 3, 0)));
        }
    }
}
