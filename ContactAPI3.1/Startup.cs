using DataOperations;
using Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Core;
using Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ContactAPI3._1
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
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IImageService, ImageService>();
            services.Configure<ImageUploadSettings>(Configuration.GetSection("ImageUploadSettings"));
            //Add Connection to Database
            services.AddDbContext<ContactContext>(options =>
               options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
               );

            //Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = Configuration["JWTSettings:Audience"],
                        ValidIssuer = Configuration["JWTSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(Configuration["JWTSettings:SecretKey"])),
                        ClockSkew = TimeSpan.Zero

                    };
                });

            //Add Identity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ContactContext>()
                .AddDefaultTokenProviders();

            //Configure IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 7;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactAPI3.1", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {

                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then valid token in the text input below. \r\n\r\nExam"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                                new string[] { }
                            }
                        });

            });
            services.AddControllers(setUp=> { setUp.ReturnHttpNotAcceptable = true; }).AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            RoleManager<IdentityRole> roleManager, UserManager<User> userManager, ContactContext context)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Seeder.Seed(roleManager, userManager, context).Wait();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactAPI3.1 v1"));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
