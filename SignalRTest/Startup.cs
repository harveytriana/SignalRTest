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
                        builder.WithOrigins("http://localhost:8018", // C# client
                                            "http://localhost:5006",
                                            "http://127.0.0.1:5000") // Django client
                               .AllowAnyHeader()
                               .AllowCredentials()
                               .AllowAnyMethod()
                               .SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });

            services.AddRazorPages();

            // NOTE. JSON is enabled by default.Adding MessagePack enables support
            //       for both JSON and MessagePack clients.
           services.AddSignalR().AddMessagePackProtocol(); ;

            // NET Core MVC Page Not Refreshing After Changes
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // Enables API REST
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
                endpoints.MapHub<WeatherReportHub>("/weatherReportHub");
                endpoints.MapControllers();
            });
        }
    }
}
