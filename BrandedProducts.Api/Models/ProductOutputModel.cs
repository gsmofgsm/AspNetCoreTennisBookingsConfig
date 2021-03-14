using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandedProducts.Api.Models
{
    public class ProductOutputModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
    }
}
