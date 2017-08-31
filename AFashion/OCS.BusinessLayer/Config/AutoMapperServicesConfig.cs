using AutoMapper;
using OCS.BusinessLayer.Models;
using OCS.DataAccess.DTO;

namespace OCS.BusinessLayer.Config
{
    public static class AutoMapperServicesConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ProductModel, Product>()
                        .ForMember(prod => prod.Name, map => map.MapFrom(p => p.Name))
                        .ForMember(prod => prod.Price, map => map.MapFrom(p => p.Price))
                        .ForMember(prod => prod.Brand, map => map.Ignore())
                        .ForMember(prod => prod.Category, map => map.Ignore())
                        .ForMember(prod => prod.Image, map => map.MapFrom(p => p.Image));

                cfg.CreateMap<Product, ProductModel>()
                        .ForMember(prod => prod.ID, map => map.MapFrom(p => p.ID))
                        .ForMember(prod => prod.Name, map => map.MapFrom(p => p.Name))
                        .ForMember(prod => prod.Price, map => map.MapFrom(p => p.Price))
                        .ForMember(prod => prod.Brand, map => map.MapFrom(p => p.Brand.Name))
                        .ForMember(prod => prod.Category, map => map.MapFrom(p => p.Category.Name))
                        .ForMember(prod => prod.Image, map => map.MapFrom(p => p.Image));
                cfg.CreateMap<Category, CategoryModel>()
                        .ForMember(cat => cat.Name, map => map.MapFrom(p => p.Name));
                cfg.CreateMap<Brand, BrandModel>()
                        .ForMember(bran => bran.Name, map => map.MapFrom(p => p.Name));
            });
        }
    }
}
