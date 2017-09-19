using Newtonsoft.Json;
using OCS.MVC.Helpers;
using OCS.MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OCS.MVC.Controllers
{
    [Authorize]
    public class ProductController : BaseController
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
        public async Task<ActionResult> ProductListPartial(string filters)
        {
            filters = HttpUtility.HtmlDecode(filters);
            filters = HttpUtility.UrlEncode(filters);
            
            var products = await GetFilteredProducts(filters);

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
            if (!ModelState.IsValid || model.Image==null)
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
        private async Task<IEnumerable<ProductViewModel>> GetFilteredProducts(string model)
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("Filter", model);

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
            byte[] imgData;
            using (MemoryStream stream = new MemoryStream())
            {
                model.Image.InputStream.CopyTo(stream);
                imgData = stream.ToArray();
            }

            CreateProductModel requestModel = new CreateProductModel
            {
                Name = model.Name,
                Price = model.Price,
                Brand = model.Brand,
                Category = model.Category,
                ImageExtension=model.Image.FileName.Substring(model.Image.FileName.LastIndexOf(".")),
                Image = imgData
            };

            HttpResponseMessage response = await HttpRequestHelper.PostAsJsonAsync("PostProduct", requestModel);

            return await response.Content.ReadAsStringAsync();
        }
        #endregion HttpClientCallers
    }
}