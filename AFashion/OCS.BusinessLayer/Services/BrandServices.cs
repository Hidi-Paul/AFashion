using AutoMapper;
using OCS.BusinessLayer.Models;
using OCS.DataAccess.DTO;
using OCS.DataAccess.Repositories;
using System.Collections.Generic;

namespace OCS.BusinessLayer.Services
{
    public class BrandServices : IBrandServices
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
