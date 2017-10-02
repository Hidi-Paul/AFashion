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
    public class ProductRepoUnitTests
    {
        private ProductRepo productRepo;
        private Mock<IFashionContext> dbContext;
        private Mock<DbSet<Product>> dbSet;
        private IQueryable<Product> testSamples;

        [SetUp]
        public void Init()
        {
            //Initializations
            testSamples = GenerateDbSet();
            dbSet = new Mock<DbSet<Product>>();
            dbSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(testSamples.Provider);
            dbSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(testSamples.Expression);
            dbSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(testSamples.ElementType);
            dbSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(testSamples.GetEnumerator());

            //QQ: This cheats the include Brand and Category Tests but allows others to work
            //Its here cuz I dont know how to solve that problem yet
            dbSet.Setup(x => x.Include(It.IsAny<string>())).Returns(dbSet.Object);

            dbContext = new Mock<IFashionContext>();
            dbContext.Setup(x => x.Products).Returns(dbSet.Object);
            
            productRepo = new ProductRepo(dbContext.Object);
        }

        private IQueryable<Product> GenerateDbSet()
        {
            List<Brand> brandTestSet = new List<Brand>()
            {
                new Brand(){ID=Guid.NewGuid(), Name="DummyBrandName0"},
                new Brand(){ID=Guid.NewGuid(), Name="DummyBrandName1"},
                new Brand(){ID=Guid.NewGuid(), Name="DummyBrandName2"},
                new Brand(){ID=Guid.NewGuid(), Name="DummyBrandName3"},
                new Brand(){ID=Guid.NewGuid(), Name="DummyBrandName4"},
            };
            List<Category> categoryTestSet = new List<Category>()
            {
                new Category(){ID=Guid.NewGuid(), Name="DummyCategoryName0"},
                new Category(){ID=Guid.NewGuid(), Name="DummyCategoryName1"},
                new Category(){ID=Guid.NewGuid(), Name="DummyCategoryName2"},
                new Category(){ID=Guid.NewGuid(), Name="DummyCategoryName3"},
                new Category(){ID=Guid.NewGuid(), Name="DummyCategoryName4"},
            };
            IEnumerable<Product> productTestSet = new List<Product>()
            {
                new Product() { ID = Guid.NewGuid(), Name = "DummyProductName0", Price=9, Image="SomeAddr/here0.jpg", Brand=brandTestSet[0], Category=categoryTestSet[0] },
                new Product() { ID = Guid.NewGuid(), Name = "DummyProductName1", Price=8, Image="SomeAddr/here1.jpg", Brand=brandTestSet[1], Category=categoryTestSet[0] },
                new Product() { ID = Guid.NewGuid(), Name = "DummyProductName2", Price=7, Image="SomeAddr/here2.jpg", Brand=brandTestSet[2], Category=categoryTestSet[1] },
                new Product() { ID = Guid.NewGuid(), Name = "DummyProductName3", Price=6, Image="SomeAddr/here3.jpg", Brand=brandTestSet[3], Category=categoryTestSet[1] },
                new Product() { ID = Guid.NewGuid(), Name = "DummyProductName4", Price=5, Image="SomeAddr/here4.jpg", Brand=brandTestSet[4], Category=categoryTestSet[2] },
                new Product() { ID = Guid.NewGuid(), Name = "dummyProductName5", Price=4, Image="SomeAddr/here5.jpg", Brand=brandTestSet[0], Category=categoryTestSet[2] },
                new Product() { ID = Guid.NewGuid(), Name = "dummyProductName6", Price=3, Image="SomeAddr/here6.jpg", Brand=brandTestSet[1], Category=categoryTestSet[3] },
                new Product() { ID = Guid.NewGuid(), Name = "dummyProductName7", Price=2, Image="SomeAddr/here7.jpg", Brand=brandTestSet[2], Category=categoryTestSet[3] },
                new Product() { ID = Guid.NewGuid(), Name = "dummyProductName8", Price=1, Image="SomeAddr/here8.jpg", Brand=brandTestSet[3], Category=categoryTestSet[4] },
                new Product() { ID = Guid.NewGuid(), Name = "dummyProductName9", Price=0, Image="SomeAddr/here9.jpg", Brand=brandTestSet[4], Category=categoryTestSet[4] }
            };
            return productTestSet.AsQueryable();
        }

        [Test]
        public void GetByID_GivenExistingProductID_ReturnsCorrectProduct()
        {
            //Arrange
            Product Product = testSamples.ElementAt(2);
            Guid id = Product.ID;

            //Act
            var result = productRepo.GetByID(id);

            //Assert
            Assert.AreEqual(Product, result);
        }

        [Test]
        public void GetByID_GivenExistingProductID_AlsoReturnsProductBrand()
        {
            //Arrange
            Product Product = testSamples.ElementAt(2);
            Guid id = Product.ID;

            //Act
            var result = productRepo.GetByID(id);

            //Assert
            Assert.AreEqual(Product.Brand, result.Brand);
        }

        [Test]
        public void GetByID_GivenExistingProductID_AlsoReturnsProductCategory()
        {
            //Arrange
            Product Product = testSamples.ElementAt(2);
            Guid id = Product.ID;

            //Act
            var result = productRepo.GetByID(id);

            //Assert
            Assert.AreEqual(Product.Category, result.Category);
        }

        [Test]
        public void GetByID_GivenNonexistantProductID_ReturnsProductNotFoundObject()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            //Act
            var result = productRepo.GetByID(id);

            //Assert
            Assert.IsTrue(result is ProductNotFound);
        }

        [Test]
        public void GetByName_GivenExistingName_ReturnsCorrectProduct()
        {
            //Arrange
            Product Product = testSamples.ElementAt(2);
            string name = Product.Name;

            //Act
            var result = productRepo.GetByName(name);

            //Assert
            Assert.AreEqual(Product, result);
        }

        [Test]
        public void GetByName_GivenExistingProductName_AlsoReturnsProductBrand()
        {
            //Arrange
            Product Product = testSamples.ElementAt(2);
            string name = Product.Name;

            //Act
            var result = productRepo.GetByName(name);

            //Assert
            Assert.AreEqual(Product.Brand, result.Brand);
        }

        [Test]
        public void GetByName_GivenExistingProductName_AlsoReturnsProductCategory()
        {
            //Arrange
            Product Product = testSamples.ElementAt(2);
            string name = Product.Name;

            //Act
            var result = productRepo.GetByName(name);

            //Assert
            Assert.AreEqual(Product.Category, result.Category);
        }
        
        [Test]
        public void GetByName_GivenNonexistantProductName_ReturnsProductNotFoundObject()
        {
            //Arrange
            string name = "Nonexistant Name";

            //Act
            var result = productRepo.GetByName(name);

            //Assert
            Assert.IsTrue(result is ProductNotFound);
        }

        [Test]
        public void GetAll_ReturnsCorrectNumberOfProducts()
        {
            //Arrange

            //Act
            var results = productRepo.GetAll();

            //Assert
            Assert.IsTrue(results.Count == testSamples.Count());
        }

        [Test]
        public void GetAll_ReturnsCorrectProducts()
        {
            //Arrange

            //Act
            var results = productRepo.GetAll();

            //Assert
            for (int i = 0; i < testSamples.Count(); i++)
            {
                Assert.AreEqual(results.ElementAt(i), testSamples.ElementAt(i));
            }
        }

        [Test]
        public void GetAll_ReturnsProductBrands()
        {
            //Arrange

            //Act
            var results = productRepo.GetAll();

            //Assert
            for (int i = 0; i < testSamples.Count(); i++)
            {
                Assert.AreEqual(results.ElementAt(i).Brand, testSamples.ElementAt(i).Brand);
            }
        }

        [Test]
        public void GetAll_ReturnsProductCategories()
        {
            //Arrange

            //Act
            var results = productRepo.GetAll();

            //Assert
            for (int i = 0; i < testSamples.Count(); i++)
            {
                Assert.AreEqual(results.ElementAt(i).Category, testSamples.ElementAt(i).Category);
            }
        }

        [Test]
        public void AddOrUpdate_GivenNewProduct_StoresItInDb()
        {
            //Arrange
            Product Product = GenerateNewProduct();
            dbSet.Setup(x => x.Add(Product)).Returns(Product);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            productRepo.AddOrUpdate(Product);

            //Assert
            dbSet.Verify(x => x.Add(Product), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        private Product GenerateNewProduct()
        {
            Guid guid = Guid.NewGuid();
            Product sampleNewProduct = new Product
            {
                ID = guid,
                Name = "RandomUniqueName",
                Price = 1337,
                Image = "SomeAddr/here.jpg",
                Brand = testSamples.ElementAt(0).Brand,
                Category = testSamples.ElementAt(1).Category
            };
            return sampleNewProduct;
        }

        [Test]
        public void AddOrUpdate_GivenNewProduct_ReturnsStoredProduct()
        {
            //Arrange
            Product product = GenerateNewProduct();
            dbSet.Setup(x => x.Add(product)).Returns(product);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            Product result = productRepo.AddOrUpdate(product);

            //Assert
            Assert.AreEqual(result, product);
        }

        [Test]
        public void AddOrUpdate_GivenExistingProduct_UpdatesItInTheDb()
        {
            //Arrange
            Product Product = testSamples.ElementAt(3);
            dbSet.Setup(x => x.Attach(Product)).Returns(Product);
            dbContext.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            productRepo.AddOrUpdate(Product);

            //Assert
            dbSet.Verify(x => x.Attach(Product), Times.Once);
            dbContext.Verify(x => x.SetModified(Product), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddOrUpdate_GivenExistingProduct_ReturnsUpdatedProduct()
        {
            //Arrange
            Product product = testSamples.ElementAt(3);
            dbSet.Setup(x => x.Attach(product)).Returns(product);
            dbContext.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            Product result = productRepo.AddOrUpdate(product);

            //Assert
            Assert.AreEqual(result, product);
        }
    }
}
