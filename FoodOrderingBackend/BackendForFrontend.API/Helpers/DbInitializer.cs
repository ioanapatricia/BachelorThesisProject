using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BackendForFrontend.API.Entities;
using BackendForFrontend.API.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackendForFrontend.API.Helpers
{
    public class DbInitializer
    {
        public static async Task SeedData(UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager, DataContext context)
        {
            //Address
            var addressData = await File.ReadAllTextAsync("Helpers/DataForSeed/Addresses.json");
            var addresses = JsonConvert.DeserializeObject<List<Address>>(addressData);

            foreach (var address in addresses)
            {
                context.Add(address);
            }

            await context.SaveChangesAsync();

            //User

            if (await userManager.Users.AnyAsync())
                return;

            var userData = await File.ReadAllTextAsync("Helpers/DataForSeed/Users.json");
            var users = JsonConvert.DeserializeObject<List<dynamic>>(userData);
            if (users == null)
                return;

            var roles = new List<AppRole>
            {
                new AppRole {Name = "Owner"},
                new AppRole {Name = "Manager"},
                new AppRole {Name = "Cook"},
                new AppRole {Name = "Driver"},
                new AppRole {Name = "Member"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var dynamicUser in users)
            {
                string streetName = dynamicUser.Street;

                var address = context.Addresses.FirstOrDefault(ad => ad.Street == streetName);

                string userName = Convert.ToString(dynamicUser.UserName);
                var user = new AppUser
                {
                    UserName = userName.ToLower(),
                    Address = address,
                    FirstName = dynamicUser.FirstName,
                    LastName = dynamicUser.LastName,
                    Email = dynamicUser.Email,
                    PhoneNumber = dynamicUser.PhoneNumber
                };

                await context.SaveChangesAsync();

                await userManager.CreateAsync(user, "password");

                var userRoles = new List<string>();

                foreach (var userRole in dynamicUser.UserRolesForSeed)
                {
                    userRoles.Add(Convert.ToString(userRole));
                }

                foreach (var role in userRoles)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                
            }
            // no need to save
        }
    }
}
