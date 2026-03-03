using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductManagement.API.Persistence;
using ProductManagement.API.Persistence.Repositories;
using ProductManagement.API.Persistence.Repositories.Interfaces;
using ProductManagement.API.Services;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators;
using ProductManagement.API.Validators.Interfaces;

namespace ProductManagement.API
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
            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContextPool<DataContext>(options =>
                options.UseSqlServer
                (
                    Configuration.GetConnectionString("DefaultConnection")
                ));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IWeightTypeRepository, WeightTypeRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();

            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IWeightTypesService, WeightTypesService>();

            services.AddScoped<IProductValidator, ProductValidator>();
            services.AddScoped<IImageValidator, ImageValidator>();
            services.AddScoped<ICategoryValidator, CategoryValidator>();

            services.AddAutoMapper(typeof(IProductRepository).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Management.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API V1");
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
