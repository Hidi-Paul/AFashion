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
    public class ShoppingCartRepoUnitTests
    {
        private ShoppingCartRepository shoppingCartRepo;
        private Mock<IFashionContext> dbContext;
        private Mock<DbSet<ShoppingCart>> dbShoppingCartSet;
        private Mock<DbSet<ProductOrder>> dbProductOrderSet;
        private IQueryable<ShoppingCart> testShoppingCartSamples;
        private IQueryable<ProductOrder> testProductOrderSamples;

        [SetUp]
        public void Init()
        {
            //Initializations
            testShoppingCartSamples = GenerateShoppingCartDbSet();
            testProductOrderSamples = GenerateProductOrderDbSet();

            dbShoppingCartSet = new Mock<DbSet<ShoppingCart>>();
            dbShoppingCartSet.As<IQueryable<ShoppingCart>>().Setup(m => m.Provider).Returns(testShoppingCartSamples.Provider);
            dbShoppingCartSet.As<IQueryable<ShoppingCart>>().Setup(m => m.Expression).Returns(testShoppingCartSamples.Expression);
            dbShoppingCartSet.As<IQueryable<ShoppingCart>>().Setup(m => m.ElementType).Returns(testShoppingCartSamples.ElementType);
            dbShoppingCartSet.As<IQueryable<ShoppingCart>>().Setup(m => m.GetEnumerator()).Returns(testShoppingCartSamples.GetEnumerator());

            dbShoppingCartSet.Setup(x => x.Include(It.IsAny<string>())).Returns(dbShoppingCartSet.Object);

            dbProductOrderSet = new Mock<DbSet<ProductOrder>>();
            dbProductOrderSet.As<IQueryable<ProductOrder>>().Setup(m => m.Provider).Returns(testProductOrderSamples.Provider);
            dbProductOrderSet.As<IQueryable<ProductOrder>>().Setup(m => m.Expression).Returns(testProductOrderSamples.Expression);
            dbProductOrderSet.As<IQueryable<ProductOrder>>().Setup(m => m.ElementType).Returns(testProductOrderSamples.ElementType);
            dbProductOrderSet.As<IQueryable<ProductOrder>>().Setup(m => m.GetEnumerator()).Returns(testProductOrderSamples.GetEnumerator());
            
            dbContext = new Mock<IFashionContext>();
            dbContext.Setup(x => x.ShoppingCarts).Returns(dbShoppingCartSet.Object);
            dbContext.Setup(x => x.ProductOrders).Returns(dbProductOrderSet.Object);

            shoppingCartRepo = new ShoppingCartRepository(dbContext.Object);
        }

        private IQueryable<ShoppingCart> GenerateShoppingCartDbSet()
        {
            IEnumerable<ShoppingCart> shoppingCartSample = new List<ShoppingCart>()
            {
                new ShoppingCart(){ ID = Guid.NewGuid(), UserName = "UsernameForUserWithCart" },
                new ShoppingCart(){ ID = Guid.NewGuid(), UserName = "DummyUser1" },
                new ShoppingCart(){ ID = Guid.NewGuid(), UserName = "DummyUser2" },
                new ShoppingCart(){ ID = Guid.NewGuid(), UserName = "DummyUser3" }
            };
            return shoppingCartSample.AsQueryable();
        }

        private IQueryable<ProductOrder> GenerateProductOrderDbSet()
        {
            List<Product> testProductSamples = new List<Product>()
            {
                new Product(){ ID = Guid.NewGuid(), Name = "DummyProductName0" },
                new Product(){ ID = Guid.NewGuid(), Name = "DummyProductName1" },
                new Product(){ ID = Guid.NewGuid(), Name = "DummyProductName2" },
                new Product(){ ID = Guid.NewGuid(), Name = "DummyProductName3" },
                new Product(){ ID = Guid.NewGuid(), Name = "DummyProductName4" }
            };
            IEnumerable<ProductOrder> testProductOrderSamples = new List<ProductOrder>()
            {
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[0], Quantity = 1, ShoppingCart = testShoppingCartSamples.ElementAt(0) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[0], Quantity = 2, ShoppingCart = testShoppingCartSamples.ElementAt(1) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[1], Quantity = 3, ShoppingCart = testShoppingCartSamples.ElementAt(1) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[2], Quantity = 4, ShoppingCart = testShoppingCartSamples.ElementAt(1) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[0], Quantity = 5, ShoppingCart = testShoppingCartSamples.ElementAt(2) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[1], Quantity = 6, ShoppingCart = testShoppingCartSamples.ElementAt(2) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[2], Quantity = 7, ShoppingCart = testShoppingCartSamples.ElementAt(2) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[3], Quantity = 8, ShoppingCart = testShoppingCartSamples.ElementAt(2) },
                new ProductOrder(){ ID = Guid.NewGuid(), Product = testProductSamples[4], Quantity = 9, ShoppingCart = testShoppingCartSamples.ElementAt(2) }
            };
            return testProductOrderSamples.AsQueryable();
        }

        [Test]
        public void AddOrUpdateShoppingCart_GivenNewShoppingCart_StoresItInDb()
        {
            //Arrange
            ShoppingCart cart = GenerateNewShoppingCart();
            dbShoppingCartSet.Setup(x => x.Add(cart)).Returns(cart);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ShoppingCart result = shoppingCartRepo.AddOrUpdateShoppingCart(cart);

            //Assert
            dbShoppingCartSet.Verify(x => x.Add(cart), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddOrUpdateShoppingCart_GivenNewShoppingCart_ReturnsStoredCart()
        {
            //Arrange
            ShoppingCart cart = GenerateNewShoppingCart();
            dbShoppingCartSet.Setup(x => x.Add(cart)).Returns(cart);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ShoppingCart result = shoppingCartRepo.AddOrUpdateShoppingCart(cart);

            //Assert
            Assert.AreEqual(result, cart);
        }

        private ShoppingCart GenerateNewShoppingCart()
        {
            ShoppingCart cart = new ShoppingCart()
            {
                ID = Guid.NewGuid(),
                UserName = "NewUserName"
            };
            return cart;
        }

        [Test]
        public void AddOrUpdateShoppingCart_GivenExistingShoppingCart_UpdatesTheDb()
        {
            //Arrange
            ShoppingCart cart = testShoppingCartSamples.ElementAt(1);
            dbShoppingCartSet.Setup(x => x.Attach(cart)).Returns(cart);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ShoppingCart result = shoppingCartRepo.AddOrUpdateShoppingCart(cart);

            //Assert
            dbShoppingCartSet.Verify(x => x.Attach(cart), Times.Once);
            dbContext.Verify(x => x.SetModified(cart), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddOrUpdateShoppingCart_GivenExistingShoppingCart_ReturnsUpdatedShoppingCart()
        {
            //Arrange
            ShoppingCart cart = testShoppingCartSamples.ElementAt(1);
            dbShoppingCartSet.Setup(x => x.Add(cart)).Returns(cart);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ShoppingCart result = shoppingCartRepo.AddOrUpdateShoppingCart(cart);

            //Assert
            Assert.AreEqual(result, cart);
        }

        [Test]
        public void AddOrUpdateProductOrder_GivenNewProductOrder_StoresItIntoDb()
        {
            //Arrange
            ProductOrder order = new ProductOrder();
            dbProductOrderSet.Setup(x => x.Add(order)).Returns(order);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ProductOrder result = shoppingCartRepo.AddOrUpdateOrder(order);

            //Assert
            dbProductOrderSet.Verify(x => x.Add(order), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddOrUpdateProductOrder_GivenNewProductOrder_ReturnsUpdatedProductOrder()
        {
            //Arrange
            ProductOrder order = new ProductOrder();
            dbProductOrderSet.Setup(x => x.Add(order)).Returns(order);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ProductOrder result = shoppingCartRepo.AddOrUpdateOrder(order);

            //Assert
            Assert.AreEqual(order, result);
        }

        private ProductOrder GenerateNewProductOrder()
        {
            Product someProduct = new Product();
            ShoppingCart someCart = new ShoppingCart();
            ProductOrder order = new ProductOrder()
            {
                ID = Guid.NewGuid(),
                Product = someProduct,
                ShoppingCart = someCart,
                Quantity = 3
            };
            return order;
        }

        [Test]
        public void AddOrUpdateProductOrder_GivenExistingProductOrder_UpdatesTheDb()
        {
            //Arrange
            ProductOrder order = testProductOrderSamples.ElementAt(1);
            dbProductOrderSet.Setup(x => x.Attach(order)).Returns(order);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ProductOrder result = shoppingCartRepo.AddOrUpdateOrder(order);

            //Assert
            dbProductOrderSet.Verify(x => x.Attach(order), Times.Once);
            dbContext.Verify(x => x.SetModified(order), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddOrUpdateProductOrder_GivenExistingProductOrder_ReturnsUpdatedProductOrder()
        {
            //Arrange
            ProductOrder order = testProductOrderSamples.ElementAt(1);
            dbProductOrderSet.Setup(x => x.Attach(order)).Returns(order);
            dbContext.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            ProductOrder result = shoppingCartRepo.AddOrUpdateOrder(order);

            //Assert
            Assert.AreEqual(order, result);
        }

        [Test]
        public void GetShoppingCartByUserName_GivenUserWithExistingCart_ReturnsThatCart()
        {
            //Arrange
            ShoppingCart cart = testShoppingCartSamples.ElementAt(0);
            string userName = cart.UserName;

            //Act
            ShoppingCart result = shoppingCartRepo.GetShoppingCartByUserName(userName);

            //Assert
            Assert.AreEqual(cart, result);
        }

        [Test]
        public void GetShoppingCartByUserName_GivenUserWithNoCart_ReturnsCartNotFound()
        {
            //Arrange
            string userName = "UsernameForUserWithNoCart";

            //Act
            ShoppingCart result = shoppingCartRepo.GetShoppingCartByUserName(userName);

            //Assert
            Assert.IsTrue(result is ShoppingCartNotFound);
        }

        [Test]
        public void DeleteOrder_GivenExistingUserWithExistingProductOrder_DeletesThatOrder()
        {
            //Arrange
            ProductOrder orderToBeDeleted = testProductOrderSamples.ElementAt(0);
            string userName = testShoppingCartSamples.ElementAt(0).UserName;

            //Act
            shoppingCartRepo.DeleteOrder(orderToBeDeleted, userName);

            //Assert
            dbContext.Verify(x => x.SetDeleted(orderToBeDeleted), Times.Once);
            dbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteOrder_GivenUsernameThatDoesNotHaveThatOrder_DoesNotDeleteThatOrder()
        {
            //Arrange
            ProductOrder order = new ProductOrder();
            string userName = testShoppingCartSamples.ElementAt(1).UserName;

            //Act
            shoppingCartRepo.DeleteOrder(order, userName);

            //Assert
            dbContext.Verify(x => x.SetDeleted(order), Times.Never);
        }

        [Test]
        public void DeleteOrder_GivenInvalidUserNameAndExistingOrder_DoesNotDeleteThatOrder()
        {
            //Arrange
            ProductOrder order = testProductOrderSamples.ElementAt(1);
            string userName = "NonExistingOrWrongUser";

            //Act
            shoppingCartRepo.DeleteOrder(order, userName);

            //Assert
            dbContext.Verify(x => x.SetDeleted(order), Times.Never);
        }
    }
}
