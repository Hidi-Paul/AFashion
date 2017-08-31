using AutoMapper;
using OCS.BusinessLayer.Filters;
using OCS.BusinessLayer.Models;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OCS.BusinessLayer.Services
{
    public class ProductServices
    {
        private readonly IEntityRepository<Product> repository;
        private readonly IEntityRepository<Brand> brandRepository;
        private readonly IEntityRepository<Category> categoryRepository;

        public ProductServices(IEntityRepository<Product> repository,
                               IEntityRepository<Brand> brandRepository,
                               IEntityRepository<Category> categoryRepository)
        {
            this.repository = repository;
            this.brandRepository = brandRepository;
            this.categoryRepository = categoryRepository;
        }

        public IEnumerable<ProductModel> GetAll()
        {
            IEnumerable<Product> productList = repository.GetAll();

            IEnumerable<ProductModel> mappedProducts = Mapper.Map<IEnumerable<ProductModel>>(productList);

            return mappedProducts;
        }

        public ProductModel GetByID(Guid id)
        {
            Product product = repository.GetByID(id);

            ProductModel mappedProduct = Mapper.Map<ProductModel>(product);

            return mappedProduct;
        }

        public void AddProduct(ProductModel productModel)
        {
            Product product = Mapper.Map<Product>(productModel);

            product.ID = Guid.NewGuid();

            var brand = brandRepository.GetByName(productModel.Brand);
            if (brand != null)
            {
                product.Brand = brand;
            }
            var categ = categoryRepository.GetByName(productModel.Category);
            if (categ != null)
            {
                product.Category = categ;
            }

            repository.AddOrUpdate(product);
            repository.SaveChanges();
        }

        public IEnumerable<ProductModel> FilteredSearch(string searchString, IEnumerable<CategoryModel> categories = null, IEnumerable<BrandModel> brands = null)
        {
            IEnumerable<Product> productList = repository.GetAll().ToList();
            AbstractFilter filter = new AbstractFilter();
            if (searchString != null)
            {
                filter = new NameFilter(productList, searchString, filter);
            }
            if (categories != null)
            {
                foreach (var categ in categories)
                {
                    filter = new CategoryFilter(productList, categ.Name, filter);
                }
            }
            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    filter = new BrandFilter(productList, brand.Name, filter);
                }
            }
            FilterResult filterResult = filter.Resolve();
            var filteredProducts = filterResult.Result();
            Mapper.Map<IEnumerable<ProductModel>>(filteredProducts);

            return Mapper.Map<IEnumerable<ProductModel>>(filteredProducts);
        }
    }
}
