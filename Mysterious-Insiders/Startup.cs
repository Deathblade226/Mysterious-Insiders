using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Mysterious_Insiders.Models;
using Mysterious_Insiders.Services;
using Microsoft.EntityFrameworkCore;


namespace Mysterious_Insiders
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<UserAccountDBContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:cdb_conn"]));
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.Configure<SheetDatabaseSettings>(Configuration.GetSection(nameof(SheetDatabaseSettings)));
            services.AddSingleton<ISheetDatabaseSettings>(s => s.GetRequiredService<IOptions<SheetDatabaseSettings>>().Value);
            services.AddSingleton<SheetService>();
            services.AddTransient<UserAccountService>();
            services.AddControllersWithViews();
            services.AddTransient(typeof(IMessageDAL), typeof(ChatWindow)); //The chat data holder
            //services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

}

}

}