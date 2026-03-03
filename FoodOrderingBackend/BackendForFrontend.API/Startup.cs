using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using BackendForFrontend.API.Persistence;
using BackendForFrontend.API.Services.BackendForFrontend;
using BackendForFrontend.API.Services.OrdersManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace BackendForFrontend.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(
                    options =>
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContextPool<DataContext>(options =>
                options.UseSqlServer
                (
                    Configuration.GetConnectionString("DefaultConnection")
                ));

            services.AddIdentityServices(Configuration);
            services.AddAutoMapper(typeof(DataContext).Assembly);
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddSingleton<IControllerHelper, ControllerHelper>();

            services.AddSingleton<IOrdersService, OrdersService>();


            services.AddExternalHttpClients(Configuration);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BFF.API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BFF API V1");
                c.RoutePrefix = "api/swagger";
            });



            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
