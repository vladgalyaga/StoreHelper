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
        private const string AllProductsCacheKey = "AllProductsCacheKey";
        private const string AllRercomendationCachekey = "AllRercomendationCachekey";

        public PurchaseSecondManager(IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _purchaseRepository = unitOfWork.GetRepository<Purchase, long>();
            _productRepository = unitOfWork.GetRepository<Product, long>();
        }

        public List<PurchaseDto> GetAllPurhase()
        {
            return _cacheService.GetOrSet(AllPurhaseCacheKey, () => _purchaseRepository.GetAll().Select(Mapper.Map).ToList());
        }

        public List<ProductDto> GetAProducts()
        {
            return _cacheService.GetOrSet(AllProductsCacheKey, () => _productRepository.GetAll().Select(Mapper.Map).ToList());
        }
        public Dictionary<long, List<RecomendationProduct>> GetRcomendation()
        {
            return _cacheService.GetOrSet(AllRercomendationCachekey, GetRecomandationValue,
                policy: new CacheItemTimePolicy(DateTime.MaxValue));
        }

        public ProductDto MakePurchase(List<long> productIds)
        {
            Purchase purchase = new Purchase()
            {
                Products = _productRepository.GetWhere(x => productIds.Contains(x.Id))
            };
            _purchaseRepository.Create(purchase);

            var recomendation = GetRcomendation();

            var recomendedProducts = new List<RecomendationProduct>();

            productIds.Where(x => recomendation.ContainsKey(x)).ToList().ForEach(x =>
                  {
                      var recomendInPurchase = recomendation[x];
                      recomendInPurchase.ForEach(r => AddRecomendation(recomendedProducts, r.ProductId, r.Value));
                  });

            var productId = recomendedProducts?
                .Where(x => !productIds.Contains(x.ProductId))?
                .OrderByDescending(x => x.Value)?
                .FirstOrDefault()?.ProductId;

            var product = _productRepository.GetFirstOrDefault(x => x.Id == productId);
            return new ProductDto()
            {
                Id = product?.Id ?? 0,
                Name = product?.Name ?? "no recomendation"
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

        private Dictionary<long, List<RecomendationProduct>> GetRecomandationValue()
        {
            var purchases = GetAllPurhase();
            Dictionary<long, List<RecomendationProduct>> recomendation = new Dictionary<long, List<RecomendationProduct>>();
            purchases.ForEach(purchase =>
            {
                var productCount = purchase.Products.Count;
                var ValueToAdd = ((double)1 / (double)productCount);
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

            var result = new Dictionary<long, List<RecomendationProduct>>();
            foreach (var r in recomendation)
            {
                result.Add(r.Key, r.Value.OrderByDescending(x => x.Value).ToList().GetRange(0, Math.Min(r.Value.Count, 10)).ToList());
            }
            return result;
        }

        public void ReloadCache()
        {
            _cacheService.Invalidate(new List<string>()
            {
                AllPurhaseCacheKey,
                AllProductsCacheKey,
                AllRercomendationCachekey
            });
            GetRcomendation();
        }

        public class RecomendationProduct
        {
            public double Value { get; set; }
            public long ProductId { get; set; }
        }


    }
}
