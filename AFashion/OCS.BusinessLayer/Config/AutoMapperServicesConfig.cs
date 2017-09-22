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
                cfg.CreateMap<CreateProductModel, ProductModel>()
                        .ForMember(src => src.ID, map => map.MapFrom(dest => dest.ID))
                        .ForMember(src => src.Name, map => map.MapFrom(dest => dest.Name))
                        .ForMember(src => src.Price, map => map.MapFrom(dest => dest.Price))
                        .ForMember(src => src.Brand, map => map.MapFrom(dest => dest.Brand))
                        .ForMember(src => src.Category, map => map.MapFrom(dest => dest.Category))
                        .ForMember(src => src.Image, map => map.Ignore());
                cfg.CreateMap<ProductModel, Product>()
                        .ForMember(src => src.Name, map => map.MapFrom(dest => dest.Name))
                        .ForMember(src => src.Price, map => map.MapFrom(dest => dest.Price))
                        .ForMember(src => src.Image, map => map.MapFrom(dest => dest.Image))
                        .ForMember(src => src.Brand, map => map.Ignore())
                        .ForMember(src => src.Category, map => map.Ignore());

                cfg.CreateMap<Product, ProductModel>()
                        .ForMember(src => src.ID, map => map.MapFrom(dest => dest.ID))
                        .ForMember(src => src.Name, map => map.MapFrom(dest => dest.Name))
                        .ForMember(src => src.Price, map => map.MapFrom(dest => dest.Price))
                        .ForMember(src => src.Brand, map => map.MapFrom(dest => dest.Brand.Name))
                        .ForMember(src => src.Category, map => map.MapFrom(dest => dest.Category.Name))
                        .ForMember(src => src.Image, map => map.MapFrom(dest => dest.Image));
                cfg.CreateMap<Category, CategoryModel>()
                        .ForMember(src => src.Name, map => map.MapFrom(dest => dest.Name));
                cfg.CreateMap<Brand, BrandModel>()
                        .ForMember(src => src.Name, map => map.MapFrom(dest => dest.Name));

                cfg.CreateMap<ProductOrder, ProductOrderModel>()
                        .ForMember(src => src.ProductName, map => map.MapFrom(dest => dest.Product.Name))
                        .ForMember(src => src.ProductQuantity, map => map.MapFrom(dest => dest.Quantity))
                        .ForMember(src => src.Image, map => map.MapFrom(dest => dest.Product.Image));
                cfg.CreateMap<ProductOrderModel, ProductOrder>()
                        .ForMember(src => src.Quantity, map => map.MapFrom(dest => dest.ProductQuantity))
                        .ForMember(src => src.ID, map => map.Ignore())
                        .ForMember(src => src.Product, map => map.Ignore())
                        .ForMember(src => src.ShoppingCart, map => map.Ignore());
            });
        }
    }
}
