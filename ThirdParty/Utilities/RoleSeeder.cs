﻿using Microsoft.AspNetCore.Identity;

namespace ThirdParty.Utilities
{
    public class RoleSeeder
    {

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Company", "Markter", "Normal" };

            foreach (var roleName in roleNames) { 
            
                    
                if(! await roleManager.RoleExistsAsync(roleName))
                {

                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            
            
            
            }



        }



    }
}
