using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTracker2020.Data;

namespace TestTracker2020.Helpers
{
    public static class DataHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            //the default connection string will come from appSetting like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //it will be automatically overwritten if we are running on heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);



        }

        public static string BuildConnectionString(string databaseUrl)
        {
            //provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            //provides a simple way to create and manage the context of connection strings used by the NpgsqlConection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };

            return builder.ToString();

        }

        public static async Task ManageData(IHost host)
        {
            try
            {
                //this technique is used to obtain references to services
                using var svcScope = host.Services.CreateScope();
                var svcProvider = svcScope.ServiceProvider;
                //var roleManager = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();//this is getting the role manager to give to the seed method
                //await ContextSeed.SeedRolesAsync(roleManager);

                //the service will run your migrations 
                var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
                await dbContextSvc.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while running Manage Data => {ex}");
            }
        }
    }
}
