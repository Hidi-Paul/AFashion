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
    public class BrandServices : IEntityServices<Brand>
    {
        private readonly IEntityRepository<Brand> repository;

        public BrandServices(IEntityRepository<Brand> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<BrandModel> GetAll()
        {
            IEnumerable<Brand> listOfBrands = repository.GetAll();
            IEnumerable<BrandModel> mappedBrands = Mapper.Map<IEnumerable<BrandModel>>(listOfBrands);

            return mappedBrands;
        }
    }
}
