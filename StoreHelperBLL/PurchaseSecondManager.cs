using Common.ViewModels;
using StoreHelper.BLL.Contracts;
using StoreHelper.Cache.Contracts;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreHelperBLL
{
    public class PurchaseSecondManager : IPurchaseManager
    {
        private IUnitOfWork _unitOfWork;
        private ICacheService _cacheService;
        private IRepository<Purchase, long> _purchaseRepository;
        private IRepository<Product, long> _productRepository;
        private const string AllPurhaseCacheKey = "AllPurhaseCacheKey";

        public PurchaseSecondManager(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _purchaseRepository = unitOfWork.GetRepository<Purchase, long>();
            _productRepository = unitOfWork.GetRepository<Product, long>();
        }

        public List<Purchase> GetAllPurhase()
        {
            return _cacheService.GetOrSet(AllPurhaseCacheKey, _purchaseRepository.GetAll);
        }

        public List<Product> GetAProducts()
        {
            return _cacheService.GetOrSet(AllPurhaseCacheKey, _productRepository.GetAll);
        }
        public Dictionary<long, List<RecomendationProduct>> GetRcomendation()
        {
            return _cacheService.GetOrSet("allRercomendationCachekey", () =>
            {
                var purchases = GetAllPurhase();
                Dictionary<long, List<RecomendationProduct>> recomendation = new Dictionary<long, List<RecomendationProduct>>();
                purchases.ForEach(purchase =>
                {
                    var productCount = purchase.Products.Count;
                    var ValueToAdd = (1 / productCount);
                    foreach (var product in purchase.Products)
                    {
                        if (!recomendation.ContainsKey(product.Id))
                            recomendation.Add(product.Id, new List<RecomendationProduct>());

                        var recomendationsOfProd = recomendation[product.Id];
                        var otherProducts = purchase.Products.Where(x => !x.Equals(product));
                        foreach (var o in otherProducts)
                        {
                            AddRecomendation(recomendationsOfProd, o.Id, ValueToAdd);
                        }
                    }
                });

                foreach (var r in recomendation)
                {
                    recomendation[r.Key] = r.Value.OrderByDescending(x => x.Value).ToList().GetRange(0, 10).ToList();
                }
                return recomendation;
            }, policy: new CacheItemTimePolicy(DateTime.MaxValue));
        }

        public ProductDto MakePurchase(List<long> productIds)
        {
            var recomendation = GetRcomendation();

            var recomendedProducts = new List<RecomendationProduct>();

            productIds.Where(x => recomendation.ContainsKey(x)).ToList().ForEach(x =>
              {
                  var recomendInPurchase = recomendation[x];
                  recomendInPurchase.ForEach(r => AddRecomendation(recomendedProducts, r.ProductId, r.Value));
              });

            var productId = recomendedProducts.OrderByDescending(x => x.Value).FirstOrDefault().ProductId;
            var product = _productRepository.GetFirstOrDefault(x => x.Id == productId);
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name
            };
        }

        private void AddRecomendation(List<RecomendationProduct> allRecomendation, long productId, double value)
        {
            var r = allRecomendation.FirstOrDefault(x => x.ProductId == productId);
            if (r == null)
            {
                allRecomendation.Add(new RecomendationProduct()
                {
                    ProductId = productId,
                    Value = value
                });
            }
            else
            {
                r.Value += value;
            }
        }

        public class RecomendationProduct
        {
            public double Value { get; set; }
            public long ProductId { get; set; }
        }


    }
}
