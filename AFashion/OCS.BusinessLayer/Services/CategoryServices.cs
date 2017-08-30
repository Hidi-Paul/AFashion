using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OCS.DataAccess.Repositories;
using OCS.DataAccess.DTO;
using OCS.BusinessLayer.Models;
using AutoMapper;

namespace OCS.BusinessLayer.Services
{
    public class CategoryServices
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
