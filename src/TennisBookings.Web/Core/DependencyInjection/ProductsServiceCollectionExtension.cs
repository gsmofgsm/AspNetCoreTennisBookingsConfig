using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TennisBookings.Web.External;

namespace TennisBookings.Web.Core.DependencyInjection
{
    public static class ProductsServiceCollectionExtension
    {
        public static IServiceCollection AddExternalProducts(this IServiceCollection services)
        {
            services.AddHttpClient<IProductsApiClient, ProductsApiClient>();

            return services;
        }
    }
}
