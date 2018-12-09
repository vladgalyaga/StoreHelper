using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ViewModels;
using StoreHelper.BLL.Contracts;
using StoreHelper.Cache.Contracts;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;

namespace StoreHelperBLL
{
    public class PurchaseManager : IPurchaseManager
    {
        private IUnitOfWork _unitOfWork;
        private ICacheService _cacheService;
        private IRepository<Purchase, int> _purchaseRepository;
        private const string AllPurhaseCacheKey = "AllPurhaseCacheKey";

        public PurchaseManager(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _purchaseRepository = unitOfWork.GetRepository<Purchase, int>();
        }

        public List<Purchase> GetAllPurhase()
        {
            return _cacheService.GetOrSet("AllPurhaseCacheKey", _purchaseRepository.GetAll);
        }

        public List<Product> GetAProducts()
        {
            return _cacheService.GetOrSet("AllProductCacheKey", _unitOfWork.GetRepository<Product, int>().GetAll);
        }

        public ProductDto MakePurchase(List<int> productIds)
        {
            Purchase purchase = new Purchase()
            {
                Products = GetAProducts().Where(x => productIds.Contains(x.Id)).ToList(),
            };
            _purchaseRepository.Create(purchase);
            var result = GetRecommendedProduct(purchase);
            result = result ?? new Product();
            return new ProductDto()
            {
                Id = result.Id,
                Name = result.Name
            };
        }

        public Product GetRecommendedProduct(Purchase purchase)
        {
            Dictionary<Product, double> recomendedPropuct = GetRecommendedProductsDictionary(purchase);

            double maxValue = 0;
            Product recomendedProduct = null;

            foreach (KeyValuePair<Product, double> keyValuePair in recomendedPropuct)
            {
                if (maxValue < keyValuePair.Value)
                {
                    maxValue = keyValuePair.Value;
                    recomendedProduct = keyValuePair.Key;
                }
            }

            return recomendedProduct;
        }

        public Dictionary<Product, double> GetRecommendedProductsDictionary(Purchase purchase)
        {

            List<Purchase> biggerPurhase = GetAllPurhase().Where(x => x.Products.Count > purchase.Products.Count).ToList();

            Dictionary<Product, double> recomendedPropucts = new Dictionary<Product, double>();


            biggerPurhase.ForEach(x =>
            {
                double similarValue = GetSimilarValue(purchase, x);
                if (similarValue > 0)
                {
                    var recomendetProducts = x.Products.Where(p => !purchase.Products.Contains(p)).ToList();
                    recomendetProducts.ForEach(p => AddSimilarValueToProduct(recomendedPropucts, p, similarValue));
                }
            });
            return recomendedPropucts;
        }

        private double GetSimilarValue(Purchase purchase1, Purchase purchase2)
        {
            List<Product> products1 = purchase1.Products.ToList();
            List<Product> products2 = purchase2.Products.ToList();

            double similarProductCount = 0;
            double divider = purchase1.Products.Count * purchase2.Products.Count;

            foreach (var prod1 in products1)
            {
                var similarProduct = products2.FirstOrDefault(x => x.Equals(prod1));
                if (similarProduct != null)
                {
                    similarProductCount++;
                    products2.Remove(similarProduct);
                }
            }

            return similarProductCount / divider;
        }

        private Dictionary<Product, double> AddSimilarValueToProduct(Dictionary<Product, double> recomendedPropucts,
            Product product, double value)
        {
            if (recomendedPropucts.ContainsKey(product))
            {
                recomendedPropucts[product] += value;
            }
            else
            {
                recomendedPropucts.Add(product, value);
            }

            return recomendedPropucts;
        }
    }
}
