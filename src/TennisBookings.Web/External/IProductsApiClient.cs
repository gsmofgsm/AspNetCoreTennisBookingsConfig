using TennisBookings.Web.External.Models;

namespace TennisBookings.Web.External
{
    public interface IProductsApiClient
    {
        System.Threading.Tasks.Task<ProductsApiResult> GetProducts();
    }
}
