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
            Product prod = productRepository.GetByID(orderModel.ProductID);

            if (prod == null)
            {
                return null;
            }

            ShoppingCart cart = GetCart(userName);
            ProductOrder order = cart.ProductOrders.Where(x => x.Product.ID.Equals(prod.ID))
                                                   .FirstOrDefault();
            if (order == null)
            {
                order = new ProductOrder()
                {
                    ID = Guid.NewGuid(),
                    Product = prod,
                    Quantity = orderModel.ProductQuantity,
                    ShoppingCart = cart
                };

            }

            var orderResult = repository.AddOrUpdateOrder(order);
            var cartResult = repository.AddOrUpdateShoppingCart(cart);
            var mappedOrder = Mapper.Map<ProductOrderModel>(order);

            return mappedOrder;
        }

        public void DeleteOrder(ProductOrderModel orderModel, string userName)
        {
            var order = repository.GetShoppingCartByUserName(userName).ProductOrders
                                                            .Where(x => x.Product.ID.Equals(orderModel.ProductID))
                                                            .FirstOrDefault();
            if (order != null)
            {
                repository.DeleteOrder(order, userName);
            }
        }

        public IEnumerable<ProductOrderModel> GetAllOrders(string userName)
        {
            var cart = GetCart(userName);
            
            IEnumerable<ProductOrder> orders = cart.ProductOrders;
            
            IEnumerable<ProductOrderModel> mappedOrders = Mapper.Map<IEnumerable<ProductOrderModel>>(orders);

            if (mappedOrders == null)
            {
                mappedOrders = new List<ProductOrderModel>();
            }

            return mappedOrders;
        }

        #region helpers
        private ShoppingCart GetCart(string userName)
        {
            ShoppingCart cart = repository.GetShoppingCartByUserName(userName);
            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    ID = Guid.NewGuid(),
                    UserName = userName
                };


                cart = repository.AddOrUpdateShoppingCart(cart);
            }
            if (cart.ProductOrders == null)
            {
                cart.ProductOrders = new List<ProductOrder>();
            }
            return cart;
        }
        #endregion helpers
    }
}
