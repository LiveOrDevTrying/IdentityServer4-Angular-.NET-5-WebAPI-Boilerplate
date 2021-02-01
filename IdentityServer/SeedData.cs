// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer
{
    public static class SeedData
    {
        public static void EnsureSeedData(
            PersistedGrantDbContext persistedGrantDbContext, 
            ConfigurationDbContext configurationDbContext, 
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager)
        {
            Console.WriteLine("Migrating and seeding database...");

            persistedGrantDbContext.Database.Migrate();
            configurationDbContext.Database.Migrate();
            applicationDbContext.Database.Migrate();

            EnsureSeedData(configurationDbContext);

            var admin = userManager.FindByNameAsync(Globals.ADMIN_USERNAME).Result;

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = Globals.ADMIN_USERNAME,
                    Email = Globals.SMS_EMAIL_SENDER_NAME
                };

                var result = userManager.CreateAsync(admin, Globals.ADMIN_PASSWORD).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, "Rob Engel"),
                    new Claim(JwtClaimTypes.GivenName, "Rob"),
                    new Claim(JwtClaimTypes.FamilyName, "Engel"),
                    new Claim(JwtClaimTypes.Email, "WebMaster@PixelHorrorStudios.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true"),
                    new Claim(JwtClaimTypes.WebSite, "https://www.pixelhorrorstudios.com.com"),
                    new Claim(JwtClaimTypes.Role, Globals.ADMIN_ROLE),
                }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Console.WriteLine("Admin created");
            }
            else
            {
                Console.WriteLine("Admin already exists");
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            foreach (var client in Config.ApiScopes)
            {
                if (!context.ApiScopes.Any(s => s.Name == client.Name))
                {
                    context.ApiScopes.Add(client.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var client in Config.Clients)
            {
                if (!context.Clients.Any(s => s.ClientId == client.ClientId))
                {
                    context.Clients.Add(client.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var identityResource in Config.IdentityResources)
            {
                if (!context.IdentityResources.Any(s => s.Name == identityResource.Name))
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var apiResource in Config.ApiResources)
            {
                if (!context.ApiResources.Any(s => s.Name == apiResource.Name))
                {
                    context.ApiResources.Add(apiResource.ToEntity());
                }
            }

            context.SaveChanges();
        }
    }
}
