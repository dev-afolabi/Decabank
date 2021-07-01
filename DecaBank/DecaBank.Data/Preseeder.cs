using DecaBank.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DecaBank.Data
{
    public class Preseeder
    {
        static string path = Directory.GetParent(Directory.GetCurrentDirectory()) + "/DecaBank.Data/Data.json/";

        private const string adminPassword = "Secret@123";
        private const string regularPassword = "P@ssw0rd";

        public async static void EnsurePopulated(IApplicationBuilder app)
        {
            //Get db context
            var _context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            if (_context.Users.Any())
            {
                return;
            }
            else
            {
                //Get Usermanager and rolemanager from IoC container
                var userManager = app.ApplicationServices.CreateScope()
                                              .ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var roleManager = app.ApplicationServices.CreateScope()
                                                .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //Create role if it doesn't exists
                var roles = new string[] { "Admin", "Customer" };
                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                //Seed Users with 1(one) Admin User
                var appUsers = GetSampleData<AppUser>(File.ReadAllText(path + "Customer.json"));
                var (adminCount, customerCount) = (0, 0);

                foreach (var user in appUsers)
                {
                    if (adminCount < 1)
                    {
                        await userManager.CreateAsync(user, adminPassword);
                        await userManager.AddToRoleAsync(user, roles[0]);
                        ++adminCount;
                    }
                    else
                    {
                        await userManager.CreateAsync(user, regularPassword);
                        await userManager.AddToRoleAsync(user, roles[1]);
                        ++customerCount;
                    }

                    ConfirmUserEmail(user, userManager);

                }
            }
        }

        //Get sample data from json files
        private static List<T> GetSampleData<T>(string location)
        {
            var output = JsonSerializer.Deserialize<List<T>>(location, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }

        //Confirm seeded user email.
        private static async void ConfirmUserEmail(AppUser user, UserManager<AppUser> userManager)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, token);
        }
    }
}
