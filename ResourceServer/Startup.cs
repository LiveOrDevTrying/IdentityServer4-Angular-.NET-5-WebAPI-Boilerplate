using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Variables;

namespace ResourceServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGlobals, Globals>();
            var globals = new Globals();

            // This is for access tokens
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = globals.IDENTITYSERVER_HTTPS_URI;
                    options.RequireHttpsMetadata = false;
                    options.Audience = globals.API_RESOURCE_NAME;
                });
            // This is for reference tokens
                //.AddOAuth2Introspection("token", options =>
                //{
                //    options.Authority = Globals.IDENTITYSERVER_URI;

                //    // this maps to the API resource name and secret
                //    options.ClientId = Globals.CLIENT_ID;
                //    options.ClientSecret = Globals.CLIENT_SECRET;
                //});

            services.AddAuthorization();

            // Cors policy to only allow requests from Angular WebApp
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins(globals.WEBAPP_URI);
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ResourceServer", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please enter Bearer Token", Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer" });
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
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            app.UseForwardedHeaders();

            // app.UseHttpsRedirection();

            app.UseRouting();

            // Cors policy to only allow requests from Angular WebApp
            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ResourceServer v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
