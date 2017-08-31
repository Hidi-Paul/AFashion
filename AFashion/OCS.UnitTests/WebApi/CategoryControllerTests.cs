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
    public class CategoryControllerTests
    {
        private CategoryController controller;
        private Mock<ICategoryServices> services;

        [SetUp]
        public void Init()
        {
            //Initializations
            services = new Mock<ICategoryServices>();

            controller = new CategoryController(services.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public void GetAllCategories_CallsProductServices()
        {
            //Arrange
            IEnumerable<CategoryModel> items = GetCategoryModelList();
            services.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllCategories();

            //Assert
            services.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAllCategories_ReturnsOkResult()
        {
            //Arrange
            IEnumerable<CategoryModel> items = GetCategoryModelList();
            services.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllCategories();

            //Assert
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<IEnumerable<CategoryModel>>), result);
        }

        [Test]
        public void GetAllCategories_ReturnsCorrectItems()
        {
            //Arrange
            IList<CategoryModel> items = GetCategoryModelList();
            services.Setup(x => x.GetAll()).Returns(items);

            //Act
            IHttpActionResult result = controller.GetAllCategories();

            //Assert
            Assert.IsNotNull(result);

            var resultContent = result as OkNegotiatedContentResult<IEnumerable<CategoryModel>>;
            Assert.IsNotNull(resultContent);
            Assert.IsNotNull(resultContent.Content);

            var resultItems = resultContent.Content as IList<CategoryModel>;
            Assert.IsTrue(resultItems.Count == items.Count);
            for (int i = 0; i < resultItems.Count; i++)
            {
                Assert.IsTrue(resultItems[i].Name.Equals(items[i].Name));
            }
        }

        #region Helpers
        private IList<CategoryModel> GetCategoryModelList()
        {
            var items = new List<CategoryModel>()
            {
                new CategoryModel(){Name="SampleCategory1"},
                new CategoryModel(){Name="SampleCategory2"},
                new CategoryModel(){Name="SampleCategory3"},
                new CategoryModel(){Name="SampleCategory4"},
                new CategoryModel(){Name="SampleCategory5"},
                new CategoryModel(){Name="SampleCategory6"}
            };
            return items;
        }
        #endregion Helpers
    }
}
