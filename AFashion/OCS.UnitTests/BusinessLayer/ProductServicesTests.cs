using Moq;
using NUnit.Framework;
using OCS.BusinessLayer.Config;
using OCS.BusinessLayer.Models;
using OCS.BusinessLayer.Services;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using OCS.DataAccess.Repositories;

namespace OCS.UnitTests.BusinessLogic
{
    [TestFixture]
    public class ProductServicesTests
    {
        //Declarations
        private ProductServices service;
        private Mock<IEntityRepository<Product>> productRepo;
        private Mock<IEntityRepository<Brand>> brandRepo;
        private Mock<IEntityRepository<Category>> categRepo;

        [SetUp]
        public void Init()
        {
            //Initializations
            AutoMapperServicesConfig.Configure();

            productRepo = new Mock<IEntityRepository<Product>>();
            brandRepo = new Mock<IEntityRepository<Brand>>();
            categRepo = new Mock<IEntityRepository<Category>>();

            service = new ProductServices(productRepo.Object, brandRepo.Object, categRepo.Object);
        }

        [Test]
        public void GetByID_ReturnsCorrectProduct()
        {
            //Arrange 
            Product productDto = GetProduct("Name", 1, "TestBrand", "TestCateg");
            Guid id = productDto.ID;
            ProductModel model = GetProductModel(productDto);

            productRepo.Setup(x => x.GetByID(id))
                   .Returns(productDto);

            //Act
            var result = service.GetByID(id);

            //Assert
            productRepo.Verify(x => x.GetByID(id), Times.Once);
            Assert.IsNotNull(result);
            Assert.IsTrue(AreEqual(model, result));
        }

        [Test]
        public void GetByID_ReturnsNullIfProductNotFound()
        {
            //Arrange 
            Guid id = new Guid();

            productRepo.Setup(x => x.GetByID(id))
                   .Returns((Product)null);

            //Act
            var result = service.GetByID(id);

            //Assert
            productRepo.Verify(x => x.GetByID(id), Times.Once);
            Assert.IsNull(result);
        }

        [Test]
        public void GetAll_CallsRepoToGetItems()
        {
            //Arrange
            List<Product> dtoList = new List<Product>()
            {
                GetProduct("Name1", 1, "TestBrand1", "TestCateg1"),
                GetProduct("Name2", 2, "TestBrand2", "TestCateg2"),
                GetProduct("Name3", 3, "TestBrand3", "TestCateg3"),
                GetProduct("Name4", 4, "TestBrand4", "TestCateg4"),
                GetProduct("Name5", 5, "TestBrand5", "TestCateg5")
            };

            productRepo.Setup(x => x.GetAll())
                   .Returns(dtoList);

            //Act
            var result = service.GetAll();

            //Assert
            productRepo.Verify(x => x.GetAll(), Times.Once);
        }

        [Test]
        public void GetAll_ReturnsListOfAllProducts()
        {
            //Arrange
            List<Product> dtoList = new List<Product>()
            {
                GetProduct("Name1", 1, "TestBrand1", "TestCateg1"),
                GetProduct("Name2", 2, "TestBrand2", "TestCateg2"),
                GetProduct("Name3", 3, "TestBrand3", "TestCateg3"),
                GetProduct("Name4", 4, "TestBrand4", "TestCateg4"),
                GetProduct("Name5", 5, "TestBrand5", "TestCateg5")
            };

            productRepo.Setup(x => x.GetAll())
                   .Returns(dtoList);

            //Act
            var result = service.GetAll();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(dtoList.Count == result.Count());
            for (int i = 0; i < dtoList.Count; i++)
            {
                Assert.IsTrue(AreEqual(GetProductModel(dtoList[i]), result.ElementAt(i)));
            }
        }

        [Test]
        public void AddProduct_CallsRepositoryToAddProduct()
        {
            //Arrange
            Product dto = GetProduct("Name1", 1, "TestBrand1", "TestCateg1");
            ProductModel model = GetProductModel(dto);

            productRepo.Setup(x => x.AddOrUpdate(It.Is<Product>(prod => AreEqual(prod, dto))));
            productRepo.Setup(x => x.SaveChanges()).Returns(1);
            categRepo.Setup(x => x.GetByName(model.Category)).Returns(dto.Category);
            brandRepo.Setup(x => x.GetByName(model.Brand)).Returns(dto.Brand);

            //Act
            service.AddProduct(model);

            //Assert
            productRepo.Verify(x => x.AddOrUpdate(It.IsAny<Product>()), Times.Once);
            productRepo.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Test]
        public void AddProduct_IncludesCategoriesRelationships()
        {
            //Arrange
            Guid categID = new Guid();

            Product dto = GetProduct("Name1", 1, "TestBrand1", "TestCateg1");
            ProductModel model = GetProductModel(dto);
            Category categ = new Category { ID = categID, Name = model.Category };

            categRepo.Setup(x => x.GetByName(model.Category)).Returns(categ);
            productRepo.Setup(x => x.AddOrUpdate(It.Is<Product>(prod => prod.Category.ID == categ.ID)));
            productRepo.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            service.AddProduct(model);

            //Assert
            categRepo.Verify(x => x.GetByName(categ.Name), Times.Once);
            productRepo.Verify(x => x.AddOrUpdate(It.IsAny<Product>()), Times.Once);
            productRepo.Verify(x => x.SaveChanges(), Times.Once);
        }
        [Test]
        public void AddProduct_IncludesBrandRelationships()
        {
            //Arrange
            Guid brandID = new Guid();

            Product dto = GetProduct("Name1", 1, "TestBrand1", "TestCateg1");
            ProductModel model = GetProductModel(dto);
            Brand brand = new Brand { ID = brandID, Name = model.Brand };

            brandRepo.Setup(x => x.GetByName(model.Brand)).Returns(brand);
            productRepo.Setup(x => x.AddOrUpdate(It.Is<Product>(prod => prod.Brand.ID == brand.ID)));
            productRepo.Setup(x => x.SaveChanges()).Returns(1);

            //Act
            service.AddProduct(model);

            //Assert
            brandRepo.Verify(x => x.GetByName(brand.Name), Times.Once);
            productRepo.Verify(x => x.AddOrUpdate(It.IsAny<Product>()), Times.Once);
            productRepo.Verify(x => x.SaveChanges(), Times.Once);
        }


        [Test]
        public void FilteredSearch_ReturnsValidResuls()
        {
            //Arrange
            string searchText = "this";
            List<CategoryModel> categs = new List<CategoryModel>()
            {
                new CategoryModel(){Name="GoodCateg1" },
                new CategoryModel(){Name="GoodCateg2" }
            };
            List<BrandModel> brands = new List<BrandModel>()
            {
                new BrandModel(){Name="GoodBrand1" },
                new BrandModel(){Name="GoodBrand2"},
                new BrandModel(){Name="GoodBrand3"}
            };

            List<Product> prods = new List<Product>()
            {
                GetProduct("this2", 2, "TestBrand2", "GoodCateg2"),
                GetProduct("this1", 1, "GoodBrand1", "GoodCateg1"),    //Valid
                GetProduct("Name3", 3, "GoodBrand3", "GoodCateg2"),
                GetProduct("this4", 4, "GoodBrand2", "TestCateg4"),
                GetProduct("Name5", 5, "TestBrand5", "TestCateg5"),
                GetProduct("3this", 5, "GoodBrand2", "GoodCateg2")      //Valid
            };
            productRepo.Setup(x => x.GetAll()).Returns(prods);
            foreach (CategoryModel filter in categs)
            {
                categRepo.Setup(x => x.GetByName(filter.Name)).Returns(new Category() { Name = filter.Name });
            }
            foreach (BrandModel filter in brands)
            {
                brandRepo.Setup(x => x.GetByName(filter.Name)).Returns(new Brand() { Name = filter.Name });
            }

            //Act
            var result = service.FilteredSearch(searchText, categs, brands).ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Count() == 2);
            Assert.IsTrue(AreEqual(result.ElementAt(0), GetProductModel(prods[1])));
            Assert.IsTrue(AreEqual(result.ElementAt(1), GetProductModel(prods[5])));
        }

        #region helpers
        private static Product GetProduct(string productName,
                                          int productPrice,
                                          string brandName,
                                          string categName,
                                          Guid id = new Guid())
        {
            Product dto = new Product
            {
                ID = id,
                Price = productPrice,
                Name = productName,
                Brand = new Brand() { Name = brandName },
                Category = new Category() { Name = categName },
                Image = "http://qwe.asd.com/zxc.jpg"
            };

            return dto;
        }
        private static ProductModel GetProductModel(Product prod)
        {
            ProductModel model = new ProductModel
            {
                ID = prod.ID,
                Name = prod.Name,
                Price = prod.Price,
                Brand = prod.Brand.Name,
                Category = prod.Category.Name,
                Image = prod.Image
            };
            return model;
        }
        private static bool AreEqual(ProductModel model, ProductModel resultModel)
        {
            return model.ID == resultModel.ID &&
                   model.Name == resultModel.Name &&
                   model.Price == resultModel.Price &&
                   model.Brand == resultModel.Brand &&
                   model.Category == resultModel.Category &&
                   model.Image == resultModel.Image;
        }
        private static bool AreEqual(Product model, Product resultModel)
        {
            return model.Name == resultModel.Name &&
                   model.Price == resultModel.Price &&
                   model.Brand.Name == resultModel.Brand.Name &&
                   model.Category.Name == resultModel.Category.Name &&
                   model.Image == resultModel.Image;
        }

        #endregion helpers
    }
}
