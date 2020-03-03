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
using Mysterious_Insiders.Hubs;

namespace Mysterious_Insiders
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<UserAccountDBContext>(opt => opt.UseSqlServer("Server=tcp:mysteriousinsiders.database.windows.net,1433;Database=useraccounts;User ID=ajen5174;Password=BbA8uCm1HSrAfP1A;Encrypt=true;Connection Timeout=30;"));
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.Configure<SheetDatabaseSettings>(Configuration.GetSection(nameof(SheetDatabaseSettings)));
            services.AddSingleton<ISheetDatabaseSettings>(s => s.GetRequiredService<IOptions<SheetDatabaseSettings>>().Value);
            services.AddSingleton<SheetService>();
            services.AddTransient<UserAccountService>();
            services.AddControllersWithViews();
            services.AddTransient(typeof(IMessageDAL), typeof(ChatWindow)); //The chat data holder
            services.AddSession();
            services.AddMvc();
            services.AddSignalR();
            
            //services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseMvc(routes =>
            {

                routes.MapRoute( //Default Page
                name: "default",
                template: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" });

            });

            app.UseEndpoints(endpoints => {
            
            endpoints.MapHub<ChatHub>("/chatHub");
            });
}

}

}