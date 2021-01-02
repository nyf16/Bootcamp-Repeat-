using AutoMapper;
using DotNetCoreIdentity.Application;
using DotNetCoreIdentity.Application.Shared;
using DotNetCoreIdentity.Domain.Identity;
using DotNetCoreIdentity.EF.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCoreIdentity.Web.Api
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
            //services.AddControllers();

            #region CORS Policy
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            #endregion

            // Veritabani adresini ef'e gonderme ve onu servislere ekleme

            services.AddDbContext<ApplicationUserDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DotNetCoreIdentityDb")
                ));
            #region Identity ve JWT konfigurasyonu

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationUserDbContext>();

            // Add Authentication - Auth/Token yonetimi
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateAudience = true,
                    ValidAudience = this.Configuration["Tokens:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = this.Configuration["Tokens:Issuer"],
                    ValidateLifetime = true
                };
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 1;
            });
            #endregion


            #region swagger config
            var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[]{ } }
                };
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc(
                    "DotNetCoreIdentityApi",
                    new OpenApiInfo
                    {
                        Title = "DotNetCoreIdentity Api Documentation",
                        Version = "0.0.1",
                        Contact = new OpenApiContact
                        {
                            Email = "info@kodluyoruz.org",
                            Name = "Kodluyoruz",                            
                        },
                        Description = "Bu bir api dokumantasyonudur!"
                    });

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Jwt Auth",
                    In = ParameterLocation.Header,
                    Name = "Authorization"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type =ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });
            #endregion

            //var jwtSecurityScheme = new OpenApiSecurityScheme
            //{
            //    Scheme = "Bearer",
            //    BearerFormat = "JWT",
            //    Name = "Kodluyoruz",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.Http,
            //    Description = "Bu bir api dokumantasyonudur!",

            //    Reference = new OpenApiReference
            //    {
            //        Id = JwtBearerDefaults.AuthenticationScheme,
            //        Type = ReferenceType.SecurityScheme
            //    }
            //};
            //s.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            //s.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {jwtSecurityScheme,Array.Empty<string>() }
            //});


            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddScoped<ICategoryService, CategoryService>();

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Cors");
            app.UseAuthentication();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger().UseSwaggerUI(u =>
            {
                u.SwaggerEndpoint("/swagger/DotNetCoreIdentityApi/swagger.json", "Swagger Test Api Endpoint");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
