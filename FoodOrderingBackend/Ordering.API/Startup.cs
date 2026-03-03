using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Ordering.API.Helpers;
using Ordering.API.Persistence;
using Ordering.API.Persistence.Interfaces;
using Ordering.API.Services;
using Ordering.API.Services.Interfaces;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IMongoClient, MongoClient>(_ => 
                new MongoClient(Configuration.GetConnectionString("MongoUrl")));

            services.AddSingleton<IOrdersRepository, OrdersRepository>();
            services.AddSingleton<IPaymentTypesRepository, PaymentTypesRepository>();
            services.AddSingleton<IStatusRepository, StatusRepository>();

            services.AddSingleton<IOrdersService, OrdersService>();
            services.AddSingleton<IPaymentTypesService, PaymentTypesService>();
            services.AddSingleton<IStatusService, StatusService>();

            services.AddAutoMapper(typeof(MapperProfiles).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering API V1");
                //c.RoutePrefix = string.Empty;
                c.RoutePrefix = "api/swagger";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
