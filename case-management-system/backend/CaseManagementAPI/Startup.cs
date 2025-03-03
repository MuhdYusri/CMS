using CaseManagementAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseManagementAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Configure services (Dependency Injection)
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Database Context
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Add controllers (API endpoints)
            services.AddControllers();

            // Add Swagger for API documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        // Configure the middleware pipeline
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}