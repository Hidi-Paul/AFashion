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
    public class BrandServicesTests
    {
        //Declarations
        private BrandServices service;
        private Mock<IEntityRepository<Brand>> repo;

        [SetUp]
        public void Init()
        {

            //All initializations
            AutoMapperServicesConfig.Configure();

            repo = new Mock<IEntityRepository<Brand>>();

            service = new BrandServices(repo.Object);
        }

        [Test]
        public void GetAll_CallsRepoToGetItems()
        {
            //Arrange
            List<Brand> dtoList = new List<Brand>()
            {
                new Brand() {Name="BrandName1"},
                new Brand() {Name="BrandName2"},
                new Brand() {Name="BrandName3"},
                new Brand() {Name="BrandName4"},
                new Brand() {Name="BrandName5"}
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
            List<Brand> dtoList = new List<Brand>()
            {
                new Brand() {Name="BrandName1"},
                new Brand() {Name="BrandName2"},
                new Brand() {Name="BrandName3"},
                new Brand() {Name="BrandName4"},
                new Brand() {Name="BrandName5"}
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
