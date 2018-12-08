using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;

namespace StoreHelper.Controllers.Api
{

    [RoutePrefix("api/Purchase")]
    public class PurchaseApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("AddPurchase")]
        public IHttpActionResult AddPurchase()
        {
            _unitOfWork.GetRepository<Product, int>().Create(new Product()
            {
                ProductType = new ProductType()
                {
                    Id =  1,
                    Name =  "nave"
                },
                Name =  "ProductName",
                Id =  1,
                Price = 23423
                
            });
            return Ok();
        }
}
}
