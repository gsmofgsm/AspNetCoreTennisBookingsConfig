using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedProducts.Api.Models
{
    public class ProductsResultOutputModel
    {
        public int TotalProducts => Products.Count;

        public IReadOnlyCollection<ProductOutputModel> Products { get; set; }
    }
}
