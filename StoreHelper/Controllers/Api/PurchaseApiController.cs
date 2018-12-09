using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreHelper.BLL.Contracts;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;

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
        
        [HttpGet]
        [Route("Make")]
        public IHttpActionResult AddPurchase(IEnumerable<int> productIds)
        {
            var result = _purchaseManager.MakePurchase(productIds.ToList());
            return Ok(result);
        }
}
}
