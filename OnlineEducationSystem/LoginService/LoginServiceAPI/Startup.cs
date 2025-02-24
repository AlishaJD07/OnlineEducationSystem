﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using LoginServiceAPI.Data;
using LoginServiceAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LoginServiceAPI
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

            // var connectionString = Configuration.GetConnectionString("LoginServiceDB");
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddControllersWithViews();

System.Console.WriteLine("\n\nThe connection string using config");
System.Console.WriteLine(Configuration["ConnectionStrings:ClassroomServiceDB"]);

services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer("Server=loginservicedb,1433;Database=LoginServiceDB;User ID=SA;Password=Pa55word!;MultipleActiveResultSets=true"));
//    options.UseSqlServer(builder.Configuration["ConnectionStrings:LoginServiceDB"]));

            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("LoginServiceDB")));

            services.AddIdentity<OnlineEducationSystemUser, OnlineEducationSystemUserRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<OnlineEducationSystemUser>()
                .AddConfigurationStore(options =>
                {
                    // options.ConfigureDbContext = builder => 
                    // builder.UseSqlServer(connectionString,    opt => opt.MigrationsAssembly(migrationsAssembly));   
                    options.ConfigureDbContext = builder => 
                        builder.UseSqlServer("Server=loginservicedb,1433;Database=LoginServiceDB;User ID=SA;Password=Pa55word!;MultipleActiveResultSets=true"
                        ,opt => opt.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    // options.ConfigureDbContext = builder => 
                    // builder.UseSqlServer(connectionString,    opt => opt.MigrationsAssembly(migrationsAssembly));
                    options.ConfigureDbContext = builder => 
                        builder.UseSqlServer("Server=loginservicedb,1433;Database=LoginServiceDB;User ID=SA;Password=Pa55word!;MultipleActiveResultSets=true"
                        ,opt => opt.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}