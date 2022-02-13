using Microsoft.EntityFrameworkCore;
using Minitwit_BE.Api.Middleware;
using Minitwit_BE.DomainService;
using Minitwit_BE.DomainService.Interfaces;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api
{
    public class Startup
    {
        public IConfiguration configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // to add services to the application container. For instance healthcheck, etc.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IFollowerDomainService, FollowerDomainService>();
            services.AddScoped<IMessageDomainService, MessageDomainService>();
            services.AddScoped<IUserDomainService, UserDomainService>();
            services.AddScoped<IPersistenceService, PersistenceService>();
            services.AddDbContext<TwitContext>(opt =>
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = Path.Join(path, "twit.db");                        // for me it's C:\Users\<USER>\AppData\Local

                opt.UseSqlite($"Data Source={dbPath}");
            });
        }

        // to configure HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();

            app.UseRouting();
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseAuthorization();

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
