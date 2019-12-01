using Common.ViewModels;
using StoreHelper.BLL.Contracts;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StoreHelper.Controllers.Api
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        private readonly IProdustsManager _produstsManager;

        public ProductsController(IProdustsManager produstsManager)
        {
            _produstsManager = produstsManager;
        }

        [HttpPost]
        [Route("list")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult AddProducts(IEnumerable<ProductDto> products)
        {
            _produstsManager.AddProducts(products.ToArray());
            return Ok();
        }

        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult AddProduct(ProductDto product)
        {
            _produstsManager.AddProducts(product);
            return Ok();
        }

        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult GetAll()
        {
            var result = _produstsManager.GetAllProtducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id:long}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Get(long id)
        {
            var result = _produstsManager.GetProduct(id);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:long}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ProductDto))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Delete(long id)
        {
             _produstsManager.Delete(id);
            return Ok();
        }
    }
}
