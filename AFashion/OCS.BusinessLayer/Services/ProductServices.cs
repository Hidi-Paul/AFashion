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
            IEnumerable<Product> listOfProducts = repository.GetAll();

            IEnumerable<ProductModel> mappedProducts = Mapper.Map<IEnumerable<ProductModel>>(listOfProducts);

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
            Product mappedProduct = Mapper.Map<Product>(productModel);

            mappedProduct.ID = Guid.NewGuid();

            var brand = brandRepository.GetByName(productModel.Brand);
            if (brand != null)
            {
                mappedProduct.Brand = brand;
            }
            var categ = categoryRepository.GetByName(productModel.Category);
            if (categ != null)
            {
                mappedProduct.Category = categ;
            }

            repository.AddOrUpdate(mappedProduct);
            repository.SaveChanges();
        }

        public IEnumerable<ProductModel> FilteredSearch(string searchString, IEnumerable<CategoryModel> categories = null, IEnumerable<BrandModel> brands = null)
        {
            IEnumerable<Product> products = repository.GetAll().ToList();
            AbstractFilter filter = new AbstractFilter();
            if (searchString != null)
            {
                filter = new NameFilter(products, searchString, filter);
            }
            if (categories != null)
            {
                foreach (var categ in categories)
                {
                    filter = new CategoryFilter(products, categ.Name, filter);
                }
            }
            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    filter = new BrandFilter(products, brand.Name, filter);
                }
            }
            FilterResult result = filter.Resolve();
            var filteredProducts = result.Result();
            Mapper.Map<IEnumerable<ProductModel>>(filteredProducts);
            return Mapper.Map<IEnumerable<ProductModel>>(filteredProducts);
        }
    }
}
