using AutoMapper;
using OCS.BusinessLayer.Filters;
using OCS.BusinessLayer.Models;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OCS.BusinessLayer.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IEntityRepository<Product> repository;
        private readonly IEntityRepository<Brand> brandRepository;
        private readonly IEntityRepository<Category> categoryRepository;

        private readonly IFileServices fileService;

        public ProductServices(IEntityRepository<Product> repository,
                               IEntityRepository<Brand> brandRepository,
                               IEntityRepository<Category> categoryRepository,
                               IFileServices fileServices)
        {
            this.repository = repository;
            this.brandRepository = brandRepository;
            this.categoryRepository = categoryRepository;
            this.fileService = fileServices;
        }

        public IEnumerable<ProductModel> GetAll()
        {
            IEnumerable<Product> productList = repository.GetAll();

            IEnumerable<ProductModel> mappedProducts = Mapper.Map<IEnumerable<ProductModel>>(productList);

            return mappedProducts;
        }

        public IEnumerable<string> GetSuggestions(string search)
        {
            IEnumerable<string> suggestions = repository.GetAll()
                                                        .Where(x=>x.Name.ToUpper().Contains(search.ToUpper()))
                                                        .Select(f => f.Name)
                                                        .ToList();

            return suggestions;
        }

        public ProductModel GetByID(Guid id)
        {
            Product product = repository.GetByID(id);

            ProductModel mappedProduct = Mapper.Map<ProductModel>(product);

            return mappedProduct;
        }

        public ProductModel AddProduct(CreateProductModel createProductModel)
        {
            ProductModel productModel = Mapper.Map<ProductModel>(createProductModel);
            Product product = Mapper.Map<Product>(productModel);
            
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

            var prod = repository.GetByName(product.Name);
            var savePath = "";
            if (prod != null)
            {
                prod.Price = product.Price;
                prod.Brand = brand;
                prod.Category = categ;

                fileService.DeleteFile(prod.Image);
                savePath = fileService.SaveFile(createProductModel.Image, prod.Name);

                product = prod;
            }
            else
            {
                product.ID = Guid.NewGuid();
                savePath=fileService.SaveFile(createProductModel.Image, product.Name+createProductModel.ImageExtension);
            }

            product.Image = savePath;
            repository.AddOrUpdate(product);

            ProductModel createdProduct = Mapper.Map<ProductModel>(product);
            return createdProduct;
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
