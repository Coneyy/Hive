using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HiveAPI.Models
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var builder = new DbContextOptionsBuilder<UserContext>();

            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Hive.Players;Trusted_Connection=True;";

            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Hive.Players;Trusted_Connection=True;");

            return new UserContext(builder.Options);
        }
    }
}
