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
    public class EntityRepositoryTests
    {
        //Declarations
        private EntityRepository<IEntity> repo;

        private Mock<FashionContext> dbCon;

        private IQueryable<IEntity> testData;
        private Mock<DbSet<IEntity>> dummyDbSet;

        [SetUp]
        public void Init()
        {
            //Initializations
            this.testData = GenerateDbSet();
            this.dummyDbSet = new Mock<DbSet<IEntity>>();
            this.dummyDbSet.As<IQueryable<IEntity>>().Setup(m => m.Provider).Returns(testData.Provider);
            this.dummyDbSet.As<IQueryable<IEntity>>().Setup(m => m.Expression).Returns(testData.Expression);
            this.dummyDbSet.As<IQueryable<IEntity>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            this.dummyDbSet.As<IQueryable<IEntity>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

            this.dbCon = new Mock<FashionContext>();
            dbCon.Setup(x => x.Set<IEntity>()).Returns(dummyDbSet.Object);

            repo = new EntityRepository<IEntity>(dbCon.Object);
        }

        [Test]
        public void GetById_ChecksDbSetForEntities()
        {
            //Arrange
            Guid guid = new Guid();

            //Act 
            var result = repo.GetByID(guid);

            //Assert
            dbCon.Verify(x => x.Set<IEntity>(), Times.Once);
        }

        [Test]
        public void GetById_ReturnsCorrectEntity()
        {
            //Arrange
            IEntity item1 = testData.ElementAt(2);

            //Act 
            var result = repo.GetByID(item1.ID);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(item1 == result);
        }

        [Test]
        public void GetById_ReturnsNullIfNotFound()
        {
            //Arrange
            Guid itemID = Guid.NewGuid();

            //Act 
            var result = repo.GetByID(itemID);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetByName_ChecksDbSetForEntities()
        {
            //Arrange
            string name = "randomName";

            //Act 
            var result = repo.GetByName(name);

            //Assert
            dbCon.Verify(x => x.Set<IEntity>(), Times.Once);
        }

        [Test]
        public void GetByName_ReturnsCorrectEntity()
        {
            //Arrange
            IEntity item1 = testData.ElementAt(2);

            //Act 
            var result = repo.GetByName(item1.Name);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(item1 == result);
        }

        [Test]
        public void GetByName_ReturnsNullIfNotFound()
        {
            //Arrange
            string name = "NoEntityWithThisName";

            //Act 
            var result = repo.GetByName(name);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetByName_IsUpperCaseInsensitive()
        {
            //Arrange
            IEntity item = testData.ElementAt(5);

            //Act 
            var result = repo.GetByName(item.Name.ToUpper());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(item == result);
        }

        [Test]
        public void GetByName_IsLowerCaseInsensitive()
        {
            //Arrange
            IEntity item = testData.ElementAt(5);

            //Act 
            var result = repo.GetByName(item.Name.ToLower());

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(item == result);
        }

        [Test]
        public void GetByName_DoesNothingWithEmptyStrings()
        {
            //Arrange
            string name = "";

            //Act 
            var result = repo.GetByName(name);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetAll_ChecksDbSetForEntities()
        {
            //Arrange

            //Act 
            var result = repo.GetAll();

            //Assert
            dbCon.Verify(x => x.Set<IEntity>(), Times.Once);
        }

        [Test]
        public void GetAll_ReturnsCorrectList()
        {
            //Arrange

            //Act 
            var result = repo.GetAll().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(testData.Count() == result.Count());
            for (int i = 0; i < testData.Count(); i++)
            {
                Assert.IsTrue(testData.ElementAt(i) == result.ElementAt(i));
            }
        }
        /*
        [Test]
        public void AddOrUpdate_ChecksDbSetForEntities()
        {
            //Arrange
            IEntity item = testData.ElementAt(7);

            //Act
            repo.AddOrUpdate(item);

            //Assert
            dbCon.Verify(x => x.Set<IEntity>(), Times.Once);
        }
        */
        /*
        [Test]
        public void AddOrUpdate_CallsUpdateIfElementExists()
        {
            //Arrange
            IEntity item = testData.ElementAt(7);

            dummyDbSet.Setup(x => x.Attach(It.IsAny<IEntity>())).Returns(item);
            dbCon.SetupSet(x => x.Entry(item).State = It.IsAny<EntityState>()).Verifiable();
            dbCon.Setup(x => x.SaveChanges()).Returns(0);
            //Act
            repo.AddOrUpdate(item);

            //Assert
            dummyDbSet.Verify(x => x.Attach(It.IsAny<IEntity>()), Times.Once);
        }
        */
        [Test]
        public void AddOrUpdate_CallsAddIfElementDoesNotExist()
        {
            //Arrange
            IEntity item = new DummyEntity() { ID = Guid.NewGuid(), Name = "NewName" };

            dummyDbSet.Setup(x => x.Add(It.IsAny<IEntity>())).Returns(item);
            dbCon.Setup(x => x.SaveChanges()).Returns(0);

            //Act
            repo.AddOrUpdate(item);

            //Assert
            dummyDbSet.Verify(x => x.Add(It.IsAny<IEntity>()), Times.Once);
        }

        [Test]
        public void AddOrUpdate_ReturnsEntityOnSuccess()
        {
            //Arrange
            IEntity item = new DummyEntity() { ID = Guid.NewGuid(), Name = "NewName" };

            dummyDbSet.Setup(x => x.Add(It.IsAny<IEntity>())).Returns(item);

            //Act
            var result = repo.AddOrUpdate(item);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(item == result);
        }

        #region Helpers
        private class DummyEntity : IEntity
        {
            public Guid ID { get; set; }
            public string Name { get; set; }
        }
        private IQueryable<IEntity> GenerateDbSet()
        {
            IEnumerable<IEntity> testSet = new List<DummyEntity>()
            {
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName0" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName1" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName2" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName3" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName4" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName5" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName6" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName7" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName8" },
                new DummyEntity() { ID = Guid.NewGuid(), Name = "DummyName9" }
            };
            return testSet.AsQueryable();
        }
        #endregion Helpers
    }
}