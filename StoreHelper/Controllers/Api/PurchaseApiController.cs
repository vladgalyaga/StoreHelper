using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Common.ViewModels;
using StoreHelper.BLL.Contracts;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;
using Swashbuckle.Swagger.Annotations;

namespace StoreHelper.Controllers.Api
{

    [RoutePrefix("api/Purchase")]
    public class PurchaseApiController : ApiController
    {
        private readonly IPurchaseManager _purchaseManager;

        public PurchaseApiController(IPurchaseManager purchaseManager)
        {
            _purchaseManager = purchaseManager;
        }
        
        [HttpPost]
        [Route("Make")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult AddPurchase(IEnumerable<long> productIds)
        {
            var result = _purchaseManager.MakePurchase(productIds.ToList());
            return Ok(result);
        }
}
}
