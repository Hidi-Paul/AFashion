using Moq;
using NUnit.Framework;
using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OCS.UnitTests.DataAccess
{
    [TestFixture]
    public class BrandRepoUnitTests
    {
        private BrandRepo brandRepo;
        private Mock<IFashionContext> dbContext;
        private Mock<DbSet<Brand>> dbSet;
        private IQueryable<Brand> testSamples;
        
        [SetUp]
        public void Init()
        {
            //Initializations
            testSamples = GenerateDbSet();
            dbSet = new Mock<DbSet<Brand>>();
            dbSet.As<IQueryable<Brand>>().Setup(m => m.Provider).Returns(testSamples.Provider);
            dbSet.As<IQueryable<Brand>>().Setup(m => m.Expression).Returns(testSamples.Expression);
            dbSet.As<IQueryable<Brand>>().Setup(m => m.ElementType).Returns(testSamples.ElementType);
            dbSet.As<IQueryable<Brand>>().Setup(m => m.GetEnumerator()).Returns(testSamples.GetEnumerator());

            dbContext = new Mock<IFashionContext>();
            dbContext.Setup(x => x.Brands).Returns(dbSet.Object);


            brandRepo = new BrandRepo(dbContext.Object);
        }

        private IQueryable<Brand> GenerateDbSet()
        {
            IEnumerable<Brand> testSet = new List<Brand>()
            {
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName0" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName1" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName2" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName3" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName4" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName5" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName6" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName7" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName8" },
                new Brand() { ID = Guid.NewGuid(), Name = "DummyName9" }
            };
            return testSet.AsQueryable();
        }

        [Test]
        public void GetByID_GivenExistingBrandID_ReturnsCorrectBrand()
        {
            //Arrange
            Brand brand = testSamples.ElementAt(2);
            Guid id = brand.ID;

            //Act
            var result = brandRepo.GetByID(id);

            //Assert
            Assert.AreEqual(brand, result);
        }

        [Test]
        public void GetByID_GivenNonexistantBrandID_ReturnsBrandNotFoundObject()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            var result = brandRepo.GetByID(id);

            //Assert
            Assert.IsTrue(result is BrandNotFound);
        }

        [Test]
        public void GetByName_GivenExistingName_ReturnsCorrectBrand()
        {
            //Arrange
            Brand brand = testSamples.ElementAt(2);
            string name = brand.Name;

            //Act
            var result = brandRepo.GetByName(name);

            //Assert
            Assert.AreEqual(brand, result);
        }

        [Test]
        public void GetByName_GivenNonexistantBrandName_ReturnsBrandNotFoundObject()
        {
            //Arrange
            string name = "Nonexistant Name";

            //Act
            var result = brandRepo.GetByName(name);

            //Assert
            Assert.IsTrue(result is BrandNotFound);
        }

        [Test]
        public void GetAll_ReturnsCorrectNumberOfBrands()
        {
            //Arrange

            //Act
            var results = brandRepo.GetAll();

            //Assert
            Assert.IsTrue(results.Count == testSamples.Count());
        }

        [Test]
        public void GetAll_ReturnsCorrectBrands()
        {
            //Arrange

            //Act
            var results = brandRepo.GetAll();

            //Assert
            for (int i = 0; i < testSamples.Count(); i++)
            {
                Assert.AreEqual(results.ElementAt(i), testSamples.ElementAt(i));
            }
        }

        [Test]
        public void AddOrUpdate_GivenNewBrand_StoresItInDb()
        {
            //Arrange
            Brand brand = GenerateNewBrand();
            dbSet.Setup(x => x.Add(brand)).Returns(brand);
            dbContext.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            brandRepo.AddOrUpdate(brand);

            //Assert
            dbSet.Verify(x => x.Add(brand), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        private Brand GenerateNewBrand()
        {
            Guid guid = Guid.NewGuid();
            Brand sampleNewBrand = new Brand
            {
                ID = guid,
                Name = "RandomUniqueName",
                Products = new List<Product>()
            };
            return sampleNewBrand;
        }

        [Test]
        public void AddOrUpdate_GivenExistingBrand_UpdatesItInTheDb()
        {
            //Arrange
            Brand brand = testSamples.ElementAt(3);
            dbSet.Setup(x => x.Attach(brand)).Returns(brand);
            dbContext.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            brandRepo.AddOrUpdate(brand);

            //Assert
            dbSet.Verify(x => x.Attach(brand), Times.Once);
            dbContext.Verify(x => x.SetModified(brand), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
