using Common.ViewModels;
using StoreHelperDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreHelperBLL
{
    public static class Mapper
    {

        public static Product Map(ProductDto productDto)
        {
            return new Product()
            {
                Id = productDto.Id,
                Name = productDto.Name,
            };
        }

        public static ProductDto Map(Product product)
        {
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
            };
        }

        public static Purchase Map(PurchaseDto purchase)
        {
            return new Purchase()
            {
                Id = purchase.Id,
                Products = purchase.Products.Select(Map).ToList()
            };
        }
        public static PurchaseDto Map(Purchase purchase)
        {
            return new PurchaseDto()
            {
                Id = purchase.Id,
                Products = purchase.Products.Select(Map).ToList()
            };
        }
    }
}
