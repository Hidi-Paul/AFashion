﻿using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using OCS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace OCS.WebApi.Controllers
{
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
        [Route("PostProduct")]
        public IHttpActionResult PostProduct([FromBody] ProductModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }
            else
            {
                try
                {
                    productServices.AddProduct(product);
                    return Created(Request.RequestUri + $"/{product.ID}", product);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
        }

        [HttpGet]
        [Route("Filter")]
        public IHttpActionResult GetFiltered([FromUri]string urlParams)
        {
            try
            {
                var model = new JavaScriptSerializer().Deserialize<FiltersModel>(urlParams);

                IEnumerable<ProductModel> products;
                if (model.Brands == null && model.Categories == null && model.SearchString == null)
                {
                    products = this.productServices.GetAll();
                }
                else
                {
                    products = this.productServices.FilteredSearch(model.SearchString, model.Categories, model.Brands);
                }
                return this.Ok(products);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
