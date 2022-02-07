namespace MinitwitBE.Api
{
    public class Startup
    {
        public IConfiguration configuration { get;}

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // to add services to the container. For instance healthcheck, etc.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // Required to have operational REST endpoints
                endpoints.MapControllers();
            });
        }
    }
}
