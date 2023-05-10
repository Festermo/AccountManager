using AccountManager.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
    public class LoginsDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public LoginsDbContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
        }

        public DbSet<AccountInfoModel> Logins => Set<AccountInfoModel>();
    }
}