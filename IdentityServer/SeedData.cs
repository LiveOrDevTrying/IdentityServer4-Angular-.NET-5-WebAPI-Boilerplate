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
using Variables;

namespace IdentityServer
{
    public static class SeedData
    {
        public static void EnsureSeedData(
            PersistedGrantDbContext persistedGrantDbContext, 
            ConfigurationDbContext configurationDbContext, 
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            IGlobals globals)
        {
            Console.WriteLine("Migrating and seeding database...");

            persistedGrantDbContext.Database.Migrate();
            configurationDbContext.Database.Migrate();
            applicationDbContext.Database.Migrate();

            EnsureSeedData(configurationDbContext, globals);

            var admin = userManager.FindByNameAsync(globals.ADMIN_USERNAME).Result;

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = globals.ADMIN_USERNAME,
                    Email = globals.ADMIN_USER_EMAIL
                };

                var result = userManager.CreateAsync(admin, globals.ADMIN_PASSWORD).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, globals.ADMIN_USER_FULL_NAME),
                    new Claim(JwtClaimTypes.GivenName, globals.ADMIN_USER_GIVEN_NAME),
                    new Claim(JwtClaimTypes.FamilyName, globals.ADMIN_USER_FAMILY_NAME),
                    new Claim(JwtClaimTypes.Email, globals.ADMIN_USER_EMAIL),
                    new Claim(JwtClaimTypes.EmailVerified, "true"),
                    new Claim(JwtClaimTypes.WebSite, globals.ADMIN_USER_WEBSITE),
                    new Claim(JwtClaimTypes.Role, globals.ADMIN_ROLE),
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

        private static void EnsureSeedData(ConfigurationDbContext context, IGlobals globals)
        {
            foreach (var client in Config.GetApiScopes(globals))
            {
                if (!context.ApiScopes.Any(s => s.Name == client.Name))
                {
                    context.ApiScopes.Add(client.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var client in Config.GetClients(globals))
            {
                if (!context.Clients.Any(s => s.ClientId == client.ClientId))
                {
                    context.Clients.Add(client.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var identityResource in Config.GetIdentityResources(globals))
            {
                if (!context.IdentityResources.Any(s => s.Name == identityResource.Name))
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var apiResource in Config.GetApiResources(globals))
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
