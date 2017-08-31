using Moq;
using NUnit.Framework;
using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using OCS.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

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
                Assert.IsTrue(resultItems[i].Name.Equals(items[i].Name));
            }
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
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleCategory1", Brand=brands[0],Category=categs[2],Price=111,Image="SomeURL:://1" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleCategory2", Brand=brands[0],Category=categs[1],Price=112,Image="SomeURL:://2" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleCategory3", Brand=brands[1],Category=categs[0],Price=121,Image="SomeURL:://3" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleCategory4", Brand=brands[1],Category=categs[0],Price=122,Image="SomeURL:://4" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleCategory5", Brand=brands[2],Category=categs[1],Price=211,Image="SomeURL:://5" },
                new ProductModel(){ID=Guid.NewGuid() ,Name="SampleCategory6", Brand=brands[2],Category=categs[2],Price=212,Image="SomeURL:://6" }
            };
            return items;
        }

        #endregion Helpers
    }
}
