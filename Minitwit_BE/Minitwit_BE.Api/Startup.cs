using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minitwit_BE.Api.Middleware;
using Minitwit_BE.DomainService;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using Prometheus;

namespace Minitwit_BE.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // to add services to the application container. For instance healthcheck,
        // etc.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<IFollowerDomainService, FollowerDomainService>();
            services.AddScoped<IMessageDomainService, MessageDomainService>();
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<ISimulationService, SimulatorService>();
            services.AddScoped<IPersistenceService, PersistenceService>();
            services.AddDbContext<TwitContext>(opt =>
            {
                //var folder = Environment.SpecialFolder.LocalApplicationData;
                //var path = Environment.GetFolderPath(folder);
                //var dbPath = Path.Join(path, "twit.db"); // for me it's C:\Users\<USER>\AppData\Local
                //opt.UseSqlite($"Data Source={dbPath}");

                opt.UseMySQL(_configuration["ConnectionStrings:TwitsDB"]);
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_miniTwitAllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:8080", "http://104.248.43.94:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ConfigurePrometheus(app);
            ConfigureHttpPipeline(app);
            ConfigureDatabase(app);
        }

        private static void ConfigurePrometheus(IApplicationBuilder app)
        {
            app.UseMetricServer();
            // Middleware Definition
            app.Use((context, next) =>
            {
                // Http Context
                var counter = Metrics.CreateCounter
                ("PathCounter", "Count request",
                new CounterConfiguration
                {
                    LabelNames = new[] { "method", "endpoint" }
                });
                // method: GET, POST etc.
                // endpoint: Requested path
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
        }

        private static void ConfigureHttpPipeline(IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline.
            // app.UseAuthorization();
            app.UseCors("_miniTwitAllowSpecificOrigins");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void ConfigureDatabase(IApplicationBuilder app)
        {
            // to create a database if it's not there
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TwitContext>();
                db.Database.EnsureCreated();
            }
        }
    }
}
