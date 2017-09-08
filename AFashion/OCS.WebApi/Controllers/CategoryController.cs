using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCS.WebApi.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class CategoryController : ApiController
    {
        private readonly ICategoryServices categServices;

        public CategoryController(ICategoryServices categServices)
        {
            this.categServices = categServices;
        }

        [HttpGet]
        [Route("GetAllCategories")]
        public IHttpActionResult GetAllCategories()
        {
            try
            {
                IEnumerable<CategoryModel> categs = categServices.GetAll();
                return this.Ok(categs);
            }
            catch (Exception e)
            {
                return this.InternalServerError(e);
            }
        }
    }
}
