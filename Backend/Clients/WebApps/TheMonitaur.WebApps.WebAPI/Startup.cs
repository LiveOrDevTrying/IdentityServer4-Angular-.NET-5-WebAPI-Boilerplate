using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheMonitaur.Domain.CodeFirst;
using TheMonitaur.Domain.Variables;

namespace TheMonitaur.WebApps.WebAPI
{
    public class Startup
    {
        public static bool IsShuttingDown;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddControllers().AddXmlDataContractSerializerFormatters();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Globals.THEMONITAUR_IDENTITYSERVER_URI;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = Globals.THEMONITAUR_WEBAPI_SCOPE;
                });

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(Globals.THEMONITAUR_WEBAPP_URI);
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact
                    {
                        Name = "The Monitaur WebMaster",
                        Email = Globals.AWS_SES_MAIL_SENDER_NAME,
                        Url = new Uri(Globals.THEMONITAUR_LANDINGPAGE_URI)
                    },
                    Description = "The WebAPI for interacting with The Monitaur Web Application",
                    Version = "v1",
                    TermsOfService = new Uri($"{Globals.THEMONITAUR_LANDINGPAGE_URI}/tos"),
                    Title = "The Monitaur WebAPI"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer" });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => c.IncludeXmlComments(xmlFile));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "The Monitaur WebAPI V1");
                c.InjectStylesheet("/css/swaggerUI.css");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        private void OnShutdown()
        {
            IsShuttingDown = true;
        }
    }
}
