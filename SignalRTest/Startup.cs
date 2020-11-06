using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRTest.Hubs;

namespace SignalRTest
{
    public class Startup
    {
        readonly string _CorsPolicy = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // CORS with named policy and middleware
            services.AddCors(options => {
                options.AddPolicy(name: _CorsPolicy,
                    builder => {
                        builder.WithOrigins("http://localhost:8018",
                                            "http://localhost:5006",
                                            "http://127.0.0.1:5000")
                               .AllowAnyHeader()
                               .AllowCredentials()
                               .AllowAnyMethod()
                               .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });

            services.AddRazorPages();
            services.AddSignalR();

            // NET Core MVC Page Not Refreshing After Changes
            // FIX stackoverflow.com/questions/53639969/net-core-mvc-page-not-refreshing-after-changes
            //     www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation/
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // api
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            // CORS with named policy and middleware
            app.UseCors(_CorsPolicy);

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<SensorHub>("/sensor");
                endpoints.MapControllers();
            });
        }
    }
}