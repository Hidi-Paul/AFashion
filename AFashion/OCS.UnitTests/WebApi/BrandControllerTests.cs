using Moq;
using NUnit.Framework;
using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using OCS.WebApi.Controllers;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace OCS.UnitTests.WebApi
{
    [TestFixture]
    public class BrandControllerTests
    {
        private BrandController controller;
        private Mock<IBrandServices> brandServices;

        [SetUp]
        public void Init()
        {
            //Initializations
            brandServices = new Mock<IBrandServices>();

            controller = new BrandController(brandServices.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public void GetAllBrands_CallsProductServices()
        {
            //Arrange
            IList<BrandModel> items = GetBrandModelList();
            brandServices.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllBrands();

            //Assert
            brandServices.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAllBrands_ReturnsOkResult()
        {
            //Arrange
            IList<BrandModel> items = GetBrandModelList();
            brandServices.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllBrands();

            //Assert
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<IEnumerable<BrandModel>>), result);
        }

        [Test]
        public void GetAllBrands_ReturnsCorrectItems()
        {
            //Arrange
            IList<BrandModel> items = GetBrandModelList();
            brandServices.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllBrands();

            //Assert
            Assert.IsNotNull(result);

            var resultContent = result as OkNegotiatedContentResult<IEnumerable<BrandModel>>;
            Assert.IsNotNull(resultContent);
            Assert.IsNotNull(resultContent.Content);

            var resultItems = resultContent.Content as IList<BrandModel>;
            Assert.IsTrue(resultItems.Count == items.Count);
            for (int i = 0; i < resultItems.Count; i++)
            {
                Assert.IsTrue(resultItems[i].Name.Equals(items[i].Name));
            }
        }

        #region Helpers
        private IList<BrandModel> GetBrandModelList()
        {
            var items = new List<BrandModel>()
            {
                new BrandModel(){Name="SampleBrand1"},
                new BrandModel(){Name="SampleBrand2"},
                new BrandModel(){Name="SampleBrand3"},
                new BrandModel(){Name="SampleBrand4"},
                new BrandModel(){Name="SampleBrand5"},
                new BrandModel(){Name="SampleBrand6"}
            };
            return items;
        }
        #endregion Helpers
    }
}
