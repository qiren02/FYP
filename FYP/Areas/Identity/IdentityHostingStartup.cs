﻿using System;
using FYP.Areas.Identity.Data;
using FYP.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FYP.Areas.Identity.IdentityHostingStartup))]
namespace FYP.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FYPContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FYPContextConnection")));

                services.AddDefaultIdentity<FYPUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<FYPContext>();
            });
        }
    }
}