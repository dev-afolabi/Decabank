using DecaBank.Data;
using DecaBank.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace DecaBank.Presentation.Extensions
{
    public static class DbContextConfiguration
    {
        private static string GetHerokuConnectionString()
        {
            // Get the Database URL from the ENV variables in Heroku
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            // parse the connection string
            if (string.IsNullOrWhiteSpace(connectionUrl))
            {
                //Use this for connection string for simulated production environment
                return "User ID=postgres;Host=localhost;Port=5432;Database=DecaBank;";
            }
            var databaseUri = new Uri(connectionUrl);
            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};" +
                   $"Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;";
        }

        public static void AddDbContextAndConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            if (env.IsProduction())
            {
                //configure postgres for Production environment
                services.AddDbContextPool<AppDbContext>(options => options.UseNpgsql(GetHerokuConnectionString()));
            }
            else
            {
                //configure postgres for Development environment
                string SqliteConnectionString = config.GetConnectionString("DefaultConnection");
                services.AddDbContextPool<AppDbContext>(options => options.UseNpgsql(SqliteConnectionString));
            }
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }
    }
}
