using Moq;
using NUnit.Framework;
using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using OCS.WebApi.Controllers;
using OCS.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;

namespace OCS.UnitTests.WebApi
{
    [TestFixture]
    public class ProductControllerTests
    {
        private ProductController controller;
        private Mock<IProductServices> services;

        [SetUp]
        public void Init()
        {
            //Initializations
            services = new Mock<IProductServices>();

            controller = new ProductController(services.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public void GetAllProducts_CallsProductServices()
        {
            //Arrange
            IEnumerable<ProductModel> items = GetProductModelList();
            services.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllProducts();

            //Assert
            services.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAllProducts_ReturnsOkResult()
        {
            //Arrange
            IEnumerable<ProductModel> items = GetProductModelList();
            services.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllProducts();

            //Assert
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<IEnumerable<ProductModel>>), result);
        }

        [Test]
        public void GetAllProducts_ReturnsCorrectItems()
        {
            //Arrange
            IList<ProductModel> items = GetProductModelList();
            services.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllProducts();

            //Assert
            Assert.IsNotNull(result);

            var resultContent = result as OkNegotiatedContentResult<IEnumerable<ProductModel>>;
            Assert.IsNotNull(resultContent);
            Assert.IsNotNull(resultContent.Content);

            var resultItems = resultContent.Content as IList<ProductModel>;
            Assert.IsTrue(resultItems.Count == items.Count);
            for (int i = 0; i < resultItems.Count; i++)
            {
                Assert.IsTrue(resultItems[i].ID.Equals(items[i].ID));
                Assert.IsTrue(resultItems[i].Name.Equals(items[i].Name));
                Assert.IsTrue(resultItems[i].Brand.Equals(items[i].Brand));
                Assert.IsTrue(resultItems[i].Category.Equals(items[i].Category));
                Assert.IsTrue(resultItems[i].Image.Equals(items[i].Image));
                Assert.IsTrue(resultItems[i].Price == items[i].Price);
            }
        }

        [Test]
        public void GetProductByID_CallsProductServices()
        {
            //Arrange
            ProductModel item = GetProductModel();

            services.Setup(x => x.GetByID(item.ID)).Returns(item);

            //Act
            IHttpActionResult result = controller.GetProductById(item.ID);

            //Assert
            services.Verify(x => x.GetByID(item.ID), Times.Once);
        }

        [Test]
        public void GetProductByID_ReturnsProductIfExists()
        {
            //Arrange
            ProductModel item = GetProductModel();

            services.Setup(x => x.GetByID(item.ID)).Returns(item);

            //Act
            IHttpActionResult result = controller.GetProductById(item.ID);

            //Assert
            Assert.IsNotNull(result);
            var resultContent = result as OkNegotiatedContentResult<ProductModel>;
            Assert.IsNotNull(resultContent.Content);
            var resultItem = resultContent.Content as ProductModel;
            Assert.IsTrue(ProductsMatch(item, resultItem));
        }

        [Test]
        public void GetProductByID_ReturnsNotFoundStatusCodeIfProductNotFound()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            services.Setup(x => x.GetByID(guid)).Returns((ProductModel)null);

            //Act
            IHttpActionResult result = controller.GetProductById(guid);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public void GetProductByID_ReturnsBadRequestIfGuidNull()
        {
            //Arrange

            //Act
            IHttpActionResult result = controller.GetProductById(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), result);
        }

        [Test]
        public void PostProduct_CallsServiceToPostProduct()
        {
            //Arrange
            var item = GetCreateProductModel();
            var itemModel = GetProductModel(item);
            services.Setup(x => x.AddProduct(item)).Returns(itemModel);

            //Act
            IHttpActionResult result = controller.PostProduct(item);

            //Assert
            services.Verify(x => x.AddProduct(It.IsAny<CreateProductModel>()), Times.Once);
        }

        [Test]
        public void PostProduct_ReturnsCreatedStatusIfProductPosted()
        {
            //Arrange
            var createItemModel=GetCreateProductModel();
            var item = GetProductModel(createItemModel);
            item.Image = "~/SomeAddr/some_file.jpg";
            services.Setup(x => x.AddProduct(createItemModel)).Returns(item);

            //Act
            IHttpActionResult result = controller.PostProduct(createItemModel);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(CreatedNegotiatedContentResult<ProductModel>), result);
            var resultContent = result as CreatedNegotiatedContentResult<ProductModel>;
            Assert.IsNotNull(resultContent.Content);
            Assert.IsTrue(ProductsMatch(item, resultContent.Content, true));
        }

        [Test]
        public void PostProduct_ReturnsBadRequestOnInvalidModel()
        {
            //Arrange

            //Act
            IHttpActionResult result = controller.PostProduct(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), result);
        }

        [Test]
        public void GetFiltered_CallsFilteringService()
        {
            //Arrange
            FiltersModel filters = new FiltersModel()
            {
                SearchString = "RandomSearchString",
                Brands = new List<BrandModel>(),
                Categories = new List<CategoryModel>()
            };
            List<ProductModel> productList = new List<ProductModel>();
            var model = new JavaScriptSerializer().Serialize(filters);

            services.Setup(x => x.FilteredSearch(filters.SearchString, filters.Categories, filters.Brands)).Returns(productList);

            //Act
            IHttpActionResult result = controller.GetFiltered(model);

            //Assert
            services.Verify(x => x.FilteredSearch(filters.SearchString, filters.Categories, filters.Brands), Times.Once);
        }

        [Test]
        public void GetFiltered_ReturnsBadRequestIfNoFilters()
        {
            //Arrange

            //Act
            IHttpActionResult result = controller.GetFiltered(null);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(BadRequestErrorMessageResult), result);
        }

        [Test]
        public void GetFiltered_ReturnsOkResultIfSucceeded()
        {
            //Arrange
            FiltersModel filters = new FiltersModel()
            {
                SearchString = "RandomSearchString",
                Brands = new List<BrandModel>(),
                Categories = new List<CategoryModel>()
            };
            ProductModel sampleProduct = GetProductModel();
            List<ProductModel> productList = new List<ProductModel>()
            {
                sampleProduct
            };

            var model = new JavaScriptSerializer().Serialize(filters);

            services.Setup(x => x.FilteredSearch(filters.SearchString, filters.Categories, filters.Brands)).Returns(productList);

            //Act
            IHttpActionResult result = controller.GetFiltered(model);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<IEnumerable<ProductModel>>), result);
            var resultContent = result as OkNegotiatedContentResult<IEnumerable<ProductModel>>;
            Assert.IsNotNull(resultContent.Content);
            var resultList = resultContent.Content as IList<ProductModel>;
            Assert.IsTrue(ProductsMatch(sampleProduct, resultList[0], true));
        }

        #region Helpers

        private IList<ProductModel> GetProductModelList()
        {
            var categs = new List<string>()
            {
                "SampleCategory1",
                "SampleCategory2",
                "SampleCategory3"
            };
            var brands = new List<string>()
            {
                "SampleBrand1",
                "SampleBrand2",
                "SampleBrand3"
            };

            var items = new List<ProductModel>()
            {
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleProduct1", Brand=brands[0],Category=categs[2],Price=111,Image="SomeURL:://1" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleProduct2", Brand=brands[0],Category=categs[1],Price=112,Image="SomeURL:://2" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleProduct3", Brand=brands[1],Category=categs[0],Price=121,Image="SomeURL:://3" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleProduct4", Brand=brands[1],Category=categs[0],Price=122,Image="SomeURL:://4" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleProduct5", Brand=brands[2],Category=categs[1],Price=211,Image="SomeURL:://5" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleProduct6", Brand=brands[2],Category=categs[2],Price=212,Image="SomeURL:://6" }
            };
            return items;
        }

        private ProductModel GetProductModel()
        {
            var item = new ProductModel()
            {
                ID = Guid.NewGuid(),
                Name = "SampleProduct1",
                Brand = "SampleBrand1",
                Category = "SampleCategory1",
                Price = 122,
                Image = "SomeURL:://4"
            };
            return item;
        }
        private CreateProductModel GetCreateProductModel()
        {
            byte[] image=null;
            var item = new CreateProductModel()
            {
                ID = Guid.NewGuid(),
                Name = "SampleProduct1",
                Brand = "SampleBrand1",
                Category = "SampleCategory1",
                Price = 122,
                Image = image
            };
            return item;
        }
        private ProductModel GetProductModel(CreateProductModel createProductModel)
        {

            var item = new ProductModel()
            {
                ID = createProductModel.ID,
                Name = createProductModel.Name,
                Brand = createProductModel.Brand,
                Category = createProductModel.Category,
                Price = createProductModel.Price
            };
            return item;
        }
        private bool ProductsMatch(ProductModel modelA, ProductModel modelB, bool ignoreIDs = false)
        {
            return (modelA.ID.Equals(modelB.ID) || ignoreIDs) &&
                   modelA.Name.Equals(modelB.Name) &&
                   modelA.Price == modelB.Price &&
                   modelA.Category.Equals(modelB.Category) &&
                   modelA.Brand.Equals(modelB.Brand) &&
                   modelA.Image.Equals(modelB.Image);
        }
        private bool ProductsMatch(CreateProductModel modelA, ProductModel modelB, bool ignoreIDs = false)
        {
            return (modelA.ID.Equals(modelB.ID) || ignoreIDs) &&
                   modelA.Name.Equals(modelB.Name) &&
                   modelA.Price == modelB.Price &&
                   modelA.Category.Equals(modelB.Category) &&
                   modelA.Brand.Equals(modelB.Brand) &&
                   modelA.Image.Equals(modelB.Image);
        }
        #endregion Helpers
    }
}
