using AutoMapper;
using OCS.BusinessLayer.Models;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IEntityRepository<Category> repository;

        public CategoryServices(IEntityRepository<Category> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            IEnumerable<Category> listOfCategs = repository.GetAll();
            IEnumerable<CategoryModel> mappedCategs = Mapper.Map<IEnumerable<CategoryModel>>(listOfCategs);

            return mappedCategs;
        }
    }
}
