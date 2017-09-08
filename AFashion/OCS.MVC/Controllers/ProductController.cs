using Newtonsoft.Json;
using OCS.MVC.Helpers;
using OCS.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OCS.MVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: All Products
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<ProductViewModel> products = await GetProducts();
            IEnumerable<CategoryViewModel> categories = await GetCategories();
            IEnumerable<BrandViewModel> brands = await GetBrands();

            var filtersViewModel = new FiltersViewModel()
            {
                SearchString = "",
                Categories = categories,
                Brands = brands
            };
            var productsPageViewModel = new ProductsPageViewModel()
            {
                FiltersViewModel = filtersViewModel,
                Products = products
            };

            return View(productsPageViewModel);
        }

        // POST: Product Filters
        [HttpPost]
        public async Task<ActionResult> ProductListPartial(FiltersViewModel model)
        {
            var products = await GetFilteredProducts(model);

            return PartialView("ProductListPartial", products);
        }

        // GET: New Product Creation Form
        [HttpGet]
        public async Task<ActionResult> AddProduct()
        {
            var brands = await GetBrands();
            var categs = await GetCategories();

            var brandNames = brands.Select(b => b.Name);
            var categNames = categs.Select(c => c.Name);


            ViewData["Brands"] = new SelectList(brandNames);
            ViewData["Categories"] = new SelectList(categNames);

            var model = new CreateProductViewModel();
            return View(model);
        }

        // POST: Add New Product
        [HttpPost]
        public async Task<ActionResult> AddProduct(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var brands = await GetBrands();
                var categs = await GetCategories();

                var brandNames = brands.Select(b => b.Name);
                var categNames = categs.Select(c => c.Name);

                ViewData["Brands"] = new SelectList(brandNames);
                ViewData["Categories"] = new SelectList(categNames);

                return View(model);
            }

            var response = await PostProduct(model);

            return RedirectToAction("Index");
        }

        #region HttpClientCallers
        private async Task<IEnumerable<ProductViewModel>> GetProducts()
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("GetAllProducts");
            List<ProductViewModel> products = new List<ProductViewModel>();
            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadAsAsync<List<ProductViewModel>>();
            }
            else
            {
                throw new ApplicationException(response.Content.ToString());
            }
            return products;
        }
        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("GetAllCategories");
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            if (response.IsSuccessStatusCode)
            {
                categories = await response.Content.ReadAsAsync<List<CategoryViewModel>>();
            }
            else
            {
                throw new ApplicationException(response.Content.ToString());
            }
            return categories;
        }
        private async Task<IEnumerable<BrandViewModel>> GetBrands()
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("GetAllBrands");
            List<BrandViewModel> brands = new List<BrandViewModel>();
            if (response.IsSuccessStatusCode)
            {
                brands = await response.Content.ReadAsAsync<List<BrandViewModel>>();
            }
            else
            {
                throw new ApplicationException(response.Content.ToString());
            }
            return brands;
        }
        private async Task<IEnumerable<ProductViewModel>> GetFilteredProducts(FiltersViewModel model)
        {
            var param = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var paramValue = param.ReadAsStringAsync().Result;
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("Filter", paramValue);

            List<ProductViewModel> products = new List<ProductViewModel>();
            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadAsAsync<List<ProductViewModel>>();
            }
            else
            {
                throw new ApplicationException(response.Content.ToString());
            }
            return products;
        }
        private async Task<string> PostProduct(CreateProductViewModel model)
        {
            HttpResponseMessage response = await HttpRequestHelper.PostAsync("PostProduct", model);

            return await response.Content.ReadAsStringAsync();
        }
        #endregion HttpClientCallers
    }
}