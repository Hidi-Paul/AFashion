using System.Collections.Generic;
using OCS.BusinessLayer.Models;
using OCS.DataAccess.Repositories;
using OCS.DataAccess.DTO;
using AutoMapper;
using System.Linq;
using System;

namespace OCS.BusinessLayer.Services
{
    public class ShoppingCartServices : IShoppingCartServices
    {
        private readonly IShoppingCartRepository repository;
        private readonly IEntityRepository<Product> productRepository;

        public ShoppingCartServices(IShoppingCartRepository repository,
                                    IEntityRepository<Product> productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
        }

        public ProductOrderModel AddOrUpdateOrder(ProductOrderModel orderModel, string userName)
        {
            Product prod = productRepository.GetByName(orderModel.ProductName);

            if (prod == null)
            {
                return null;
            }

            ShoppingCart cart = repository.GetByUserName(userName);
            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    ID = new Guid(),
                    UserName = userName
                };
                cart = repository.AddOrUpdate(cart);
            }

            ProductOrder order = cart.ProductOrders.Where(x => x.Product.ID.Equals(prod.ID))
                                                   .FirstOrDefault();
            if (order == null)
            {
                order = new ProductOrder()
                {
                    ID = new Guid(),
                    Product = prod,
                    Quantity = orderModel.ProductQuantity,
                    ShoppingCart = cart
                };
            }

            var result = repository.AddOrUpdate(cart);
            var mappedOrder = Mapper.Map<ProductOrderModel>(result);

            return mappedOrder;
        }

        public void DeleteOrder(ProductOrderModel orderModel, string userName)
        {
            var order = repository.GetByUserName(userName).ProductOrders
                                                            .Where(x => x.Product.Name.Equals(orderModel.ProductName))
                                                            .FirstOrDefault();
            if (order != null)
            {
                repository.Delete(order, userName);
            }
        }

        public IEnumerable<ProductOrderModel> GetAllOrders(string userName)
        {
            var cart = repository.GetByUserName(userName);

            IEnumerable<ProductOrder> orders = cart.ProductOrders;

            IEnumerable<ProductOrderModel> mappedOrders = Mapper.Map<IEnumerable<ProductOrderModel>>(orders);

            return mappedOrders;
        }
    }
}
