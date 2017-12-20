using HiveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HiveAPI.Data
{
    public class HiveApiContext : DbContext
    {
        public HiveApiContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        
    }
}
