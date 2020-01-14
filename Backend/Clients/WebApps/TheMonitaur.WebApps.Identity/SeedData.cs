using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheMonitaur.Domain.CodeFirst;
using TheMonitaur.Domain.Variables;

namespace TheMonitaur.WebApps.Identity
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<IdentityContext>().Database.Migrate();
                
                EnsureSeedData(scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>());

                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<Domain.CodeFirst.Models.Identity>>();

                var admin = userMgr.FindByNameAsync(Globals.THEMONITAUR_ADMIN_USERNAME).Result;

                if (admin == null)
                {
                    admin = new Domain.CodeFirst.Models.Identity
                    {
                        UserName = Globals.THEMONITAUR_ADMIN_USERNAME,
                        Email = Globals.AWS_SES_MAIL_SENDER_NAME
                    };

                    var result = userMgr.CreateAsync(admin, Globals.THEMONITAUR_ADMIN_PASSWORD).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(admin, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "Rob Engel"),
                        new Claim(JwtClaimTypes.GivenName, "Rob"),
                        new Claim(JwtClaimTypes.FamilyName, "Engel"),
                        new Claim(JwtClaimTypes.Email, "WebMaster@TheMonitaur.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true"),
                        new Claim(JwtClaimTypes.WebSite, "https://www.TheMonitaur.com"),
                        new Claim(JwtClaimTypes.Role, Globals.THEMONITAUR_ADMIN_ROLE),
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
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            foreach (var client in Config.GetClients().ToList())
            {
                if (!context.Clients.Any(s => s.ClientId == client.ClientId))
                {
                    context.Clients.Add(client.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var identityResource in Config.GetIdentityResources().ToList())
            {
                if (!context.IdentityResources.Any(s => s.Name == identityResource.Name))
                {
                    context.IdentityResources.Add(identityResource.ToEntity());
                }
            }
            context.SaveChanges();

            foreach (var apiResource in Config.GetApiResources().ToList())
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
