using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OCS.WebApi.Controllers
{
    [Authorize]
    [EnableCors("*", "*", "*")]
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCartServices cartServices;

        public ShoppingCartController(IShoppingCartServices cartServices)
        {
            this.cartServices = cartServices;
        }

        [HttpGet]
        [Route("GetAllOrders")]
        public IHttpActionResult GetAllOrders()
        {
            string user = User.Identity.Name;
            if (user == null || user.Length == 0)
            {
                return Unauthorized();
            }

            IEnumerable<ProductOrderModel> orders = cartServices.GetAllOrders(user);

            return Ok(orders);
        }

        [HttpPost]
        [Route("DeleteOrder")]
        public IHttpActionResult DeleteOrder(ProductOrderModel model)
        {
            string user = User.Identity.Name;
            if (user == null || user.Length == 0)
            {
                return Unauthorized();
            }

            cartServices.DeleteOrder(model, user);

            return this.NotFound();
        }

        [HttpPost]
        [Route("AddOrder")]
        public IHttpActionResult AddOrder(ProductOrderModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Order");
            }

            string user = User.Identity.Name;
            if (user == null || user.Length == 0)
            {
                return Unauthorized();
            }

            ProductOrderModel item = cartServices.AddOrUpdateOrder(model, user);
            return Created(Request.RequestUri, item);
        }
    }
}
