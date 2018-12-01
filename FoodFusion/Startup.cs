using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.ConcurrentEvents;
using DataAccess.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Authentication;
using Services.Authentication.Models;
using Services.Authorization;
using Services.Employees;
using Services.Menus;
using Services.Reservations;
using Services.Restaurants;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Middlewares;

namespace FoodFusion
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
            services.AddCors();

            services.AddAutoMapper();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Food Fusion API", Version = "v1" });
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });
            
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddAuthorization();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddDbContext<FoodFusionContext>(options =>
                    options.UseSqlServer(Configuration.GetSection("ConnectionStrings:FoodFusionDB").Value)
                        .ConfigureWarnings(warning => warning.Ignore(CoreEventId.IncludeIgnoredWarning)),
                    optionsLifetime: ServiceLifetime.Scoped);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtKeyString = Configuration.GetSection("Jwt:SymetricSecurityKey").Value;
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKeyString));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };

                options.RequireHttpsMetadata = false;
                //options.SaveToken = true;
            });
            
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IHasher, Hasher>();

            services.AddTransient<IRestaurantService, RestaurantService>();

            services.AddTransient<IMenuService, MenuService>();

            services.AddTransient<IEmployeesService, EmployeesService>();

            services.AddTransient<IReservationsService, ReservationsService>();
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            services.AddTransient(typeof(IConcurrentEventsService<>), typeof(ConcurrentEventsService<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<FoodFusionContext>();
                context.Database.Migrate();
            }
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCustomExceptionMiddleware();

            app.UseCors(builder =>
                builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodFusion API V1");
            });

        }
    }
}
