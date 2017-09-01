using NUnit.Framework;
using OCS.BusinessLayer.Filters;
using OCS.DataAccess.DTO;
using System.Collections.Generic;
using System.Linq;

namespace OCS.UnitTests.BusinessLayer.Filters
{
    [TestFixture]
    public class FilterResultTests
    {
        private FilterResult filterResult;

        [SetUp]
        public void Init()
        {
            //Initializations
            filterResult = new FilterResult();
        }

        [Test]
        public void AddFilter_AddsFilterToDictionary()
        {
            //Arrange
            string key1 = "key1";
            string key2 = "key2";
            string key3 = "key3";
            var filter1 = new List<Product>();
            var filter2 = new List<Product>();
            var filter3 = new List<Product>();

            //Act
            filterResult.AddFilter(key1, filter1);
            filterResult.AddFilter(key2, filter2);
            filterResult.AddFilter(key3, filter3);

            //Assert
            Assert.NotNull(filterResult.Filters);
            Assert.IsNotEmpty(filterResult.Filters);
            Assert.IsTrue(filterResult.Filters.ContainsKey(key1));
            Assert.IsTrue(filterResult.Filters[key1] == filter1);
            Assert.IsTrue(filterResult.Filters.ContainsKey(key2));
            Assert.IsTrue(filterResult.Filters[key2] == filter2);
            Assert.IsTrue(filterResult.Filters.ContainsKey(key3));
            Assert.IsTrue(filterResult.Filters[key3] == filter3);
        }

        [Test]
        public void AddFilter_SameFilterResultsDoNotOverwrite()
        {
            //Arrange
            string key1 = "key1";
            Product prod1 = new Product();
            Product prod2 = new Product();
            var filter1 = new List<Product>() { prod1 };
            var filter2 = new List<Product>() { prod2 };

            //Act
            filterResult.AddFilter(key1, filter1);
            filterResult.AddFilter(key1, filter2);

            //Assert
            Assert.IsTrue(filterResult.Filters.Count == 1);
            Assert.IsTrue(filterResult.Filters.ContainsKey(key1));
            Assert.IsTrue(filterResult.Filters[key1].Count() == 2);
            Assert.IsTrue(filterResult.Filters[key1].Contains(prod1));
            Assert.IsTrue(filterResult.Filters[key1].Contains(prod2));
        }

        [Test]
        public void Result_ReturnsEmptyListIfNoFilters()
        {
            //Arrange

            //Act
            var result = filterResult.Result();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void Result_ReturnsFilteredProducts()
        {
            //Arrange
            var items = new List<Product>()
            {
                new Product(),
                new Product()
            };

            //Act
            filterResult.AddFilter("someKey", items);
            var result = filterResult.Result();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 2);
            Assert.IsTrue(items.All(result.Contains));
            Assert.IsTrue(result.All(items.Contains));
        }

        [Test]
        public void Result_ReturnsValidResultsWithStackingFilters()
        {
            //Arrange
            var commonProduct = new Product();
            var items1 = new List<Product>()
            {
                new Product(),
                commonProduct
            };
            var items2 = new List<Product>()
            {
                commonProduct,
                new Product()
            };

            //Act
            filterResult.AddFilter("someKey1", items1);
            filterResult.AddFilter("someKey2", items2);
            var result = filterResult.Result();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() == 1);
            Assert.IsTrue(result.Contains(commonProduct));
        }
    }
}
