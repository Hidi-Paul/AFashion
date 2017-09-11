using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using OCS.WebApi.Attributes;
using OCS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace OCS.WebApi.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class ProductController : ApiController
    {
        private readonly IProductServices productServices;

        public ProductController(IProductServices productServices)
        {
            this.productServices = productServices;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IHttpActionResult GetAllProducts()
        {
            try
            {
                IEnumerable<ProductModel> products = productServices.GetAll();
                return this.Ok(products);
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }

        [HttpGet]
        [Route("GetProductByID")]
        public IHttpActionResult GetProductById(Guid? id)
        {
            if (id == null)
            {
                return BadRequest("Please specify the product Id");
            }
            try
            {
                ProductModel product = this.productServices.GetByID((Guid)id);
                if (product != null)
                {
                    return this.Ok(product);
                }
                return this.NotFound();
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }

        [HttpPost]
        [AuthorizeClaim("Role","Admin")]
        [Route("PostProduct")]
        public IHttpActionResult PostProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid || product == null)
            {
                return BadRequest("Invalid data.");
            }
            try
            {
                var productID = productServices.AddProduct(product);
                return Created(Request.RequestUri + $"/{productID}", product);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpGet]
        [Route("Filter")]
        public IHttpActionResult GetFiltered([FromUri]string urlParams)
        {
            if (urlParams==null||urlParams.Length==0)
            {
                return BadRequest("Invalid data.");
            }
            var model = new JavaScriptSerializer().Deserialize<FiltersModel>(urlParams);

            IEnumerable<ProductModel> products;
            if(model.SearchString == null && model.Categories==null && model.Brands == null)
            {
                return GetAllProducts();
            }

            products = this.productServices.FilteredSearch(model.SearchString, model.Categories, model.Brands);

            return this.Ok(products);
        }
    }
}
