using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore.Internal;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataOperations
{
    public class Seeder
    {
        public  async static Task  Seed(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ContactContext context)
        {
            await context.Database.EnsureCreatedAsync();
            if (!context.Users.Any())
            {
               
                List<string> roles = new List<string> { "Admin", "Regular" };

                foreach(var role in roles)
                {
                   await roleManager.CreateAsync(new IdentityRole { Name = role });

                }

                List<User> users = new List<User>
                {
                    new User
                    {
                        FirstName = "John",
                        LastName = "James",
                        Email = "john@gmail.com",
                        UserName = "johnny",
                        PhoneNumber = "8765235674"

                    },

                     new User
                     {
                        FirstName = "Jane",
                        LastName = "Janet",
                        Email = "jane@gmail.com",
                        UserName = "jenny",
                        PhoneNumber = "080998976"

                     },

                      new User
                      {
                        FirstName = "peter",
                        LastName = "paul",
                        Email = "peter@gmail.com",
                        UserName = "pero",
                        PhoneNumber = "0985435789"

                      },
                };

                foreach(var user in users)
                {
                   await userManager.CreateAsync(user, "Chidori@07");
                    if (user == users[0])
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "Regular");
                    }
                 
                }
            }
        }
    }
}
