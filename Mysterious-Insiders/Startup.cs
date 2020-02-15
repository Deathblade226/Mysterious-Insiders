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

namespace Mysterious_Insiders {
public class Startup {
public Startup(IConfiguration configuration) {
    Configuration = configuration;
}

public IConfiguration Configuration { get; }

// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services) { services.AddMvc(option => option.EnableEndpointRouting = false); }

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
    app.UseStaticFiles();
    

    app.UseMvc(routes => {

    routes.MapRoute( //Default Page
    name: "default",
    template: "",
    defaults: new { controller = "Home", action = "Index"}
    );
    
    routes.MapRoute(
    name: "namedCows",
    template: "{total:int?}/{sides:int?}/{mod:int?}/{allRolls:bool?}",
    defaults: new {controller = "Home", action = "DiceRoll"}
    );

    });

}

}

}
