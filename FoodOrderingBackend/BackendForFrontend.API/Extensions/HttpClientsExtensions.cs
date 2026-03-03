using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace BackendForFrontend.API.Extensions
{
    public static class HttpClientsExtensions
    {
        public static IServiceCollection AddExternalHttpClients(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddHttpClient(HttpClientsEnum.ProductManagementApi, client =>
                {
                    client.BaseAddress = new Uri(config["PRODUCT_MANAGEMENT_API_BASE_URL"]);
                })
                .AddTransientHttpErrorPolicy(x =>
                    x.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));


            services.AddHttpClient(HttpClientsEnum.OrderingApi, client =>
                {
                    client.BaseAddress = new Uri(config["ORDERING_API_BASE_URL"]);
                })
                .AddTransientHttpErrorPolicy(x =>
                    x.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            services.AddHttpClient(HttpClientsEnum.SiteManagementApi, client =>
                {
                    client.BaseAddress = new Uri(config["SITE_MANAGEMENT_API_BASE_URL"]);
                })
                .AddTransientHttpErrorPolicy(x =>
                    x.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(300)));

            return services;
        }
    }
}
    