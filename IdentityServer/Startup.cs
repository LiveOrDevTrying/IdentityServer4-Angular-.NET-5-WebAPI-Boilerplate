// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Variables;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGlobals, Globals>();
            var globals = new Globals();

             var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(globals.CONNECTION_STRING_IDENTITY));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // Cors is handled by Client configuration

            // Uncomment if you would like auto redirection after logout enabled
            //AccountOptions.AutomaticRedirectAfterSignOut = true;

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                // Override this if forwarding headers are unique
                //options.IssuerUri = globals.IDENTITYSERVER_URI;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddAspNetIdentity<ApplicationUser>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(globals.CONNECTION_STRING_IDENTITY,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(globals.CONNECTION_STRING_IDENTITY,
                            sql => sql.MigrationsAssembly(migrationsAssembly));

                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 300;
                });

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                // ** IMPORTANT **
                // Required for Production! - Make sure to add cert.pfx into the wwwroot folder
                var filename = Path.Combine(Environment.WebRootPath, "cert.pfx");

                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException("Signing Certificate is missing!");
                }

                var cert = new X509Certificate2(filename, globals.CERTIFICATE_PASSWORD);
                builder.AddSigningCredential(cert);
            }

            // Uncomment for requested 3rd party auth
            // May not need SignInScheme if cookie is not desired (and login required on each visit of IdentityServer)
            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.ClientId = "";
            //        options.ClientSecret = "";
            //    })
            //    .AddTwitch(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.ClientId = "";
            //        options.ClientSecret = "";
            //    })
            //    .AddFacebook(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.ClientId = "";
            //        options.ClientSecret = "";
            //    })
            //    .AddTwitter(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.ConsumerKey = "";
            //        options.ConsumerSecret = "";
            //    })
            //    .AddMicrosoftAccount(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.ClientId = "";
            //        options.ClientSecret = "";
            //    });
        }

        public void Configure(IApplicationBuilder app,
            PersistedGrantDbContext persistedGrantDbContext,
            ConfigurationDbContext configurationDbContext,
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            IGlobals globals)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            SeedData.EnsureSeedData(persistedGrantDbContext, configurationDbContext, applicationDbContext, userManager, globals);

            app.UseForwardedHeaders();

            // ** IMPORTANT **
            // If using Http for IdentityServer (SSL terminated before Asp.NET), SameSiteMode.Strict or SameSiteMode.Lax MUST be set or 
            // the IdentityServer will not redirect aftert login.
            //app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

            // This can be uncommented if running SSL termination at the Asp.NET app.
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseRouting();

            // Cors is handled by Client configuration

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}