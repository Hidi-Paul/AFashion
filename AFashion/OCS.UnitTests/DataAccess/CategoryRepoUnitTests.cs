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
    public class categoryRepoUnitTests
    {
        private CategoryRepo categoryRepo;
        private Mock<IFashionContext> dbContext;
        private Mock<DbSet<Category>> dbSet;
        private IQueryable<Category> testSamples;


        [SetUp]
        public void Init()
        {
            //Initializations
            testSamples = GenerateDbSet();
            dbSet = new Mock<DbSet<Category>>();
            dbSet.As<IQueryable<Category>>().Setup(m => m.Provider).Returns(testSamples.Provider);
            dbSet.As<IQueryable<Category>>().Setup(m => m.Expression).Returns(testSamples.Expression);
            dbSet.As<IQueryable<Category>>().Setup(m => m.ElementType).Returns(testSamples.ElementType);
            dbSet.As<IQueryable<Category>>().Setup(m => m.GetEnumerator()).Returns(testSamples.GetEnumerator());

            dbContext = new Mock<IFashionContext>();
            dbContext.Setup(x => x.Categories).Returns(dbSet.Object);


            categoryRepo = new CategoryRepo(dbContext.Object);
        }

        private IQueryable<Category> GenerateDbSet()
        {
            IEnumerable<Category> testSet = new List<Category>()
            {
                new Category() { ID = Guid.NewGuid(), Name = "DummyName0" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName1" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName2" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName3" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName4" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName5" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName6" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName7" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName8" },
                new Category() { ID = Guid.NewGuid(), Name = "DummyName9" }
            };
            return testSet.AsQueryable();
        }

        [Test]
        public void GetByID_GivenExistingCategoryID_ReturnsCorrectCategory()
        {
            //Arrange
            Category category = testSamples.ElementAt(2);
            Guid id = category.ID;

            //Act
            var result = categoryRepo.GetByID(id);

            //Assert
            Assert.AreEqual(category, result);
        }

        [Test]
        public void GetByID_GivenNonexistantCategoryID_ReturnsCategoryNotFoundObject()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            var result = categoryRepo.GetByID(id);

            //Assert
            Assert.IsTrue(result is CategoryNotFound);
        }

        [Test]
        public void GetByName_GivenExistingName_ReturnsCorrectCategory()
        {
            //Arrange
            Category category = testSamples.ElementAt(2);
            string name = category.Name;

            //Act
            var result = categoryRepo.GetByName(name);

            //Assert
            Assert.AreEqual(category, result);
        }

        [Test]
        public void GetByName_GivenNonexistantCategoryName_ReturnsCategoryNotFoundObject()
        {
            //Arrange
            string name = "Nonexistant Name";

            //Act
            var result = categoryRepo.GetByName(name);

            //Assert
            Assert.IsTrue(result is CategoryNotFound);
        }

        [Test]
        public void GetAll_ReturnsCorrectNumberOfCategories()
        {
            //Arrange

            //Act
            var results = categoryRepo.GetAll();

            //Assert
            Assert.IsTrue(results.Count == testSamples.Count());
        }

        [Test]
        public void GetAll_ReturnsCorrectCategories()
        {
            //Arrange

            //Act
            var results = categoryRepo.GetAll();

            //Assert
            for (int i = 0; i < testSamples.Count(); i++)
            {
                Assert.AreEqual(results.ElementAt(i), testSamples.ElementAt(i));
            }
        }

        [Test]
        public void AddOrUpdate_GivenNewCategory_StoresItInDb()
        {
            //Arrange
            Category category = GenerateNewCategory();
            dbSet.Setup(x => x.Add(category)).Returns(category);
            dbContext.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            categoryRepo.AddOrUpdate(category);

            //Assert
            dbSet.Verify(x => x.Add(category), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        private Category GenerateNewCategory()
        {
            Guid guid = Guid.NewGuid();
            Category sampleNewCategory = new Category
            {
                ID = guid,
                Name = "RandomUniqueName",
                Products = new List<Product>()
            };
            return sampleNewCategory;
        }

        [Test]
        public void AddOrUpdate_GivenExistingCategory_UpdatesItInTheDb()
        {
            //Arrange
            Category category = testSamples.ElementAt(3);
            dbSet.Setup(x => x.Attach(category)).Returns(category);
            dbContext.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            categoryRepo.AddOrUpdate(category);

            //Assert
            dbSet.Verify(x => x.Attach(category), Times.Once);
            dbContext.Verify(x => x.SetModified(category), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
