using OCS.MVC.Helpers;
using OCS.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OCS.MVC.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public async Task<ActionResult> Index(string msg = "")
        {
            var orders = await GetAllOrders();

            if (msg.Length > 0)
            {
                ViewBag.Message = msg;
            }

            return View(orders);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteOrder(Guid? model)
        {
            if (model == null)
            {
                return View();
            }

            OrderViewModel order = new OrderViewModel()
            {
                ProductID = (Guid)model,
                ProductQuantity = 0
            };

            await Delete(order);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> PostProduct(Guid? model)
        {
            if (model == null)
            {
                return View();
            }

            OrderViewModel order = new OrderViewModel()
            {
                ProductID = (Guid)model,
                ProductQuantity = 1
            };

            string result = await PostOrder(order);

            return RedirectToAction("Index");
        }





        #region HttpClientCallers
        private async Task<IEnumerable<OrderViewModel>> GetAllOrders()
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("GetAllOrders");

            List<OrderViewModel> orders = new List<OrderViewModel>();
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<List<OrderViewModel>>();
            }
            else
            {
                throw new ApplicationException(response.Content.ToString());
            }
            return orders;
        }
        private async Task Delete(OrderViewModel model)
        {
            HttpResponseMessage response = await HttpRequestHelper.PostAsJsonAsync("DeleteOrder", model);
        }
        private async Task<string> PostOrder(OrderViewModel model)
        {
            HttpResponseMessage response = await HttpRequestHelper.PostAsJsonAsync("AddOrder", model);

            return await response.Content.ReadAsStringAsync();
        }



        #endregion HttpClientCallers
    }
}