using Moq;
using NUnit.Framework;
using OCS.DataAccess.Context;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System.Data.Entity;

namespace OCS.UnitTests.DataAccess
{
    [TestFixture]
    public class BrandRepoUnitTests
    {
        private BrandRepo brandRepo;
        private Mock<IFashionContext> dbContext;
        private Mock<DbSet<Brand>> dbSet;

        [SetUp]
        public void Init()
        {
            //Initializations
            dbContext = new Mock<IFashionContext>();

            brandRepo = new BrandRepo(dbContext.Object);
        }

        [Test]
        public void AddOrUpdate_GivenNewBrand_StoresItInDb()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}
