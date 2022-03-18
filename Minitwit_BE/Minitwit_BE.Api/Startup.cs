using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Api.Middleware;
using Minitwit_BE.DomainService;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;
using Prometheus;

namespace Minitwit_BE.Api
{
    public class Startup
    {
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
        }

        // to configure HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("_miniTwitAllowSpecificOrigins");
            app.UseStaticFiles();

            // Prometheus setup start
            app.UseMetricServer();
            // Middleware Definition
            app.Use((context, next) =>
            {
                // Http Context
                var counter = Metrics.CreateCounter(
                    "PathCounter", "Count request",
                    new CounterConfiguration
                    {
                        LabelNames = new[] { "method", "endpoint" }
                    });
                // method: GET, POST etc.
                // endpoint: Requested path
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
            // Prometheus setup end

            app.UseRouting(); 
            app.UseMiddleware<ExceptionMiddleware>();

            // app.UseAuthorization();

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
