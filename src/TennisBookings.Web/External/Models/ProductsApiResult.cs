using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisBookings.Web.External.Models
{
    public class ProductsApiResult
    {
        public int TotalProducts { get; set; }

        public IReadOnlyCollection<Product> Products { get; set; }
    }
}
