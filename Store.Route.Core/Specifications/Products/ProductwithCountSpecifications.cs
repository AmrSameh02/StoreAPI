using Store.Route.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Route.Core.Specifications.Products
{
    public class ProductwithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductwithCountSpecifications(ProductSpecParams productSpec)
            : base(
                 p =>
                          (string.IsNullOrEmpty(productSpec.Search) || p.Name.ToLower().Contains(productSpec.Search))
                          &&
                          (!productSpec.BrandId.HasValue || productSpec.BrandId == p.BrandId)
                          &&
                          (!productSpec.TypeId.HasValue || productSpec.TypeId == p.TypeId)
                  )
        {


        }

    }
}
