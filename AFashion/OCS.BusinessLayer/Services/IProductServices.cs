using OCS.BusinessLayer.Models;
using System;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Services
{
    public interface IProductServices
    {
        IEnumerable<ProductModel> GetAll();

        ProductModel GetByID(Guid id);

        void AddProduct(ProductModel productModel);

        IEnumerable<ProductModel> FilteredSearch(string searchString, IEnumerable<CategoryModel> categories, IEnumerable<BrandModel> brands);
    }
}
