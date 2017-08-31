using Moq;
using NUnit.Framework;
using OCS.BusinessLayer.Config;
using OCS.BusinessLayer.Services;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace OCS.UnitTests.BusinessLogic
{
    [TestFixture]
    public class CategortServicesTests
    {
        //Declarations
        private CategoryServices service;
        private Mock<IEntityRepository<Category>> repo;

        [SetUp]
        public void Init()
        {

            //All initializations
            AutoMapperServicesConfig.Configure();

            repo = new Mock<IEntityRepository<Category>>();

            service = new CategoryServices(repo.Object);
        }

        [Test]
        public void GetAll_CallsRepoToGetItems()
        {
            //Arrange
            List<Category> dtoList = new List<Category>()
            {
                new Category() {Name="CategName1"},
                new Category() {Name="CategName2"},
                new Category() {Name="CategName3"},
                new Category() {Name="CategName4"},
                new Category() {Name="CategName5"}
            };

            repo.Setup(x => x.GetAll())
                   .Returns(dtoList);

            //Act
            var result = service.GetAll();

            //Assert
            repo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAll_ReturnsCorrectItems()
        {
            //Arrange
            List<Category> dtoList = new List<Category>()
            {
                new Category() {Name="CategName1"},
                new Category() {Name="CategName2"},
                new Category() {Name="CategName3"},
                new Category() {Name="CategName4"},
                new Category() {Name="CategName5"}
            };

            repo.Setup(x => x.GetAll())
                   .Returns(dtoList);

            //Act
            var result = service.GetAll();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(dtoList.Count == result.Count());
            for (int i = 0; i < dtoList.Count; i++)
            {
                Assert.IsTrue(dtoList[i].Name == result.ElementAt(i).Name);
            }
        }
    }
}
