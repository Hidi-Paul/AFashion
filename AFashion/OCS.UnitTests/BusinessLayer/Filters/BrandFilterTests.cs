using Moq;
using NUnit.Framework;
using OCS.BusinessLayer.Filters;
using OCS.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OCS.UnitTests.BusinessLayer.Filters
{
    [TestFixture]
    public class BrandFilterTests
    {
        [SetUp]
        public void Init()
        {
            //Initializations

        }

        [Test]
        public void Resolve_FiltersCorrectly()
        {
            //Arrange
            var items = GetProductList();
            var filterBrand = items[5].Brand;
            var filter = new BrandFilter(items, filterBrand.Name);

            var goodItems = new List<Product>();
            for (int i = 0; i < items.Count(); i++)
            {
                if (items[i].Brand == filterBrand)
                {
                    goodItems.Add(items[i]);
                }
            }

            //Act
            var result = filter.Resolve();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Filters);
            Assert.IsTrue(result.Filters.ContainsKey("Brand"));
            var filteredItems = result.Filters["Brand"] as IList<Product>;
            Assert.IsTrue(goodItems.All(filteredItems.Contains));
            Assert.IsTrue(filteredItems.All(goodItems.Contains));
        }

        [Test]
        public void Resolve_StacksWithOtherFilters()
        {
            //Arrange

            //Mock another Filter
            var secondFilterItems = new List<Product>();
            var secondFilterResults = new FilterResult();
            secondFilterResults.AddFilter("secondFilter", secondFilterItems);

            var secondFilter = new Mock<AbstractFilter>();
            secondFilter.Setup(x => x.Resolve()).Returns(secondFilterResults);

            var filter = new BrandFilter(new List<Product>(), "SomeFilter", secondFilter.Object);

            //Act
            var result = filter.Resolve();

            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Filters);
            Assert.IsTrue(result.Filters.ContainsKey("Brand"));
            Assert.IsTrue(result.Filters.ContainsKey("secondFilter"));
        }

        #region Helpers
        private IList<Product> GetProductList()
        {
            var brands = new List<Brand>()
            {
                new Brand{ID=Guid.NewGuid(), Name="SampleBrand1"},
                new Brand{ID=Guid.NewGuid(), Name="SampleBrand2"},
                new Brand{ID=Guid.NewGuid(), Name="SampleBrand3"}
            };

            var categs = new List<Category>()
            {
                new Category{ID=Guid.NewGuid(), Name="SampleCategory1"},
                new Category{ID=Guid.NewGuid(), Name="SampleCategory2"},
                new Category{ID=Guid.NewGuid(), Name="SampleCategory3"}
            };

            var items = new List<Product>()
            {
                new Product{ID=Guid.NewGuid(), Name="SampleProduct00", Image="http://someImage.jpg", Price=10, Brand=brands[0], Category=categs[0]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct01", Image="http://someImage.jpg", Price=11, Brand=brands[0], Category=categs[1]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct02", Image="http://someImage.jpg", Price=12, Brand=brands[0], Category=categs[2]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct03", Image="http://someImage.jpg", Price=13, Brand=brands[0], Category=categs[0]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct04", Image="http://someImage.jpg", Price=14, Brand=brands[1], Category=categs[1]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct05", Image="http://someImage.jpg", Price=15, Brand=brands[1], Category=categs[2]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct06", Image="http://someImage.jpg", Price=16, Brand=brands[1], Category=categs[0]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct07", Image="http://someImage.jpg", Price=17, Brand=brands[1], Category=categs[1]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct08", Image="http://someImage.jpg", Price=18, Brand=brands[1], Category=categs[2]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct09", Image="http://someImage.jpg", Price=19, Brand=brands[2], Category=categs[0]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct10", Image="http://someImage.jpg", Price=20, Brand=brands[2], Category=categs[1]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct11", Image="http://someImage.jpg", Price=21, Brand=brands[2], Category=categs[2]},
                new Product{ID=Guid.NewGuid(), Name="SampleProduct12", Image="http://someImage.jpg", Price=22, Brand=brands[2], Category=categs[0]},
            };
            return items;
        }
        #endregion Helpers
    }
}
