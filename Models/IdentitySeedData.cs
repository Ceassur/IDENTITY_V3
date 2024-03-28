using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY_V2.Models
{
    public class IdentitySeedData
    {
        public const string adminUser ="Admin";
        public const string adminPassword ="Tnsdc48.";

        public static async void IdentityTestUser(IApplicationBuilder app){
           
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            if(context.Database.GetAppliedMigrations().Any()){
                context.Database.Migrate();
            }

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var user = await userManager.FindByNameAsync(adminUser);

            if(user == null){

                user = new AppUser();

                user.Email = "deneme@deneme.com";
                user.UserName = adminUser;
                user.PhoneNumber = "5427606754";
                user.FullName = "Hakan Hakyemez";
                user.IsActive = true; 

            await userManager.CreateAsync(user,adminPassword);
     

        }
        
    }
}
}