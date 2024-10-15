using Microsoft.AspNetCore.Identity;
using Store.Route.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Repository.Identity
{
    public class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager)
        {
            if(_userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    Email = "amrsameh999@gmail.com",
                    DisplayName = "Amr Sameh",
                    UserName = "Amr.Sameh",
                    PhoneNumber = "01102224",
                    Address = new Address()
                    {
                        FName = "Amr",
                        LName = "Sameh",
                        City = "Sharkia",
                        Country = "Egypt",
                        Street = "Nazah"
                    }
                };
                await _userManager.CreateAsync(user, "P@ssW0rd"); ;
            }

        }
    }
}
