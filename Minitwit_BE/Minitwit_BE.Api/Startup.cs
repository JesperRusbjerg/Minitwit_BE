using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Api.Middleware;
using Minitwit_BE.DomainService;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using Prometheus;
using Prometheus.SystemMetrics;

namespace Minitwit_BE.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // to add services to the application container. For instance healthcheck, etc.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<IFollowerDomainService, FollowerDomainService>();
            services.AddScoped<IMessageDomainService, MessageDomainService>();
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<ISimulationService, SimulatorService>();
            services.AddScoped<IPersistenceService, PersistenceService>();



            string connectionString = "Server=mariadb;Database=WaystoneInn;Uid=root;Pwd=SuperSecretPassword;";
            services.AddDbContext<TwitContext>(
                options => options.UseMySql(
                    connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddCors(options =>
            {
                options.AddPolicy(name: "_miniTwitAllowSpecificOrigins", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:8080", "http://159.89.13.60:8080")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddSystemMetrics();
        }

        // configures HTTP request middleware pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("_miniTwitAllowSpecificOrigins");
            app.UseStaticFiles();

            // Prometheus setup start
            app.UseMetricServer();
            app.UseHttpMetrics();
            // Middleware Definition
            Histogram responseTime = Metrics.CreateHistogram(
                "responseTime",
                "The time it takes for the server to process a request");
            app.Use((context, next) =>
            {
                using (responseTime.NewTimer())
                {
                    var counter = Metrics.CreateCounter(
                        "PathCounter", "Count request",
                        new CounterConfiguration
                        {
                            LabelNames = new[] { "method", "endpoint" }
                        });
                    counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                    return next();
                }
            });
            // Prometheus setup end

            app.UseRouting();
            app.UseMiddleware<LogContextMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                // Required to have operational REST endpoints
                endpoints.MapControllers();
            });

            // to create a database if it's not there
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TwitContext>();
                db.Database.EnsureCreated();
            }
        }
    }
}
