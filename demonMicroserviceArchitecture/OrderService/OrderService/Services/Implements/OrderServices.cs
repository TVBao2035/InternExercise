using System.Text.Json;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderService.Common;
using OrderService.Models;
using OrderService.Models.DTOs;
using OrderService.Models.Enities;
using OrderService.Repositories.Implements;
using OrderService.Repositories.Interfaces;
using OrderService.Services.Interfaces;

namespace OrderService.Services.Implements
{
    public class OrderServices : IOrderService
    {
        private IOrderRepository _orderRepository;
        private static HttpClient httpClient = new HttpClient();
        public OrderServices(IOrderRepository orderRepository) {
            _orderRepository = orderRepository;
        }
        public async Task<AppReponse<OrderDTO>> Create(OrderDTO request)
        {
            var result = new AppReponse<OrderDTO>();
            try
            {
                string httpUserString = $"http://localhost:5114/api/User/{request.UserId}";
                string httpProductString = $"http://localhost:5031/api/Product/{request.ProductId}";
               
              
                AppHttpClient<AppReponse<User>> userHttp = new AppHttpClient<AppReponse<User>>();
                var user = await userHttp.GetDataFromHttp(httpUserString);
                if (user.StatusCode != 200) 
                    return result.SendReponse(user.StatusCode, user.Message);

                AppHttpClient<AppReponse<Product>> productHttp = new AppHttpClient<AppReponse<Product>>();
                var product = await productHttp.GetDataFromHttp(httpProductString);
                if (product.StatusCode != 200) 
                    return result.SendReponse(product.StatusCode, product.Message);
                Order order = new Order();
                order.Id = Guid.NewGuid();
                order.UserId = request.UserId;
                order.ProductId = request.ProductId;
                _orderRepository.Add(order);

                return result.SendReponse(200, "Success", request);
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async Task<AppReponse<OrderDTO>> Delete(Guid Id)
        {
            var result = new AppReponse<OrderDTO>();
            try
            {
                return result.SendReponse(200, "Success");
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async Task<AppReponse<List<OrderDTO>>> GetAll()
        {
            var result = new AppReponse<List<OrderDTO>>();
            try
            {
                string httpUserString;
                string httpProductString;

               
                var listOrder = await _orderRepository.Query()
                    .Select(or => new OrderDTO
                    {
                        UserId = or.UserId,
                        ProductId = or.ProductId,
                    }).ToListAsync();

                AppHttpClient<AppReponse<User>> userHttp = new AppHttpClient<AppReponse<User>>();
                AppHttpClient<AppReponse<Product>> productHttp = new AppHttpClient<AppReponse<Product>>();

                foreach(var order in listOrder)
                {
                    httpUserString = $"http://localhost:5114/api/User/{order.UserId}";
                    httpProductString = $"http://localhost:5031/api/Product/{order.ProductId}";

                    var user = await userHttp.GetDataFromHttp(httpUserString);
                    if (user.StatusCode != 200)
                        return result.SendReponse(user.StatusCode, user.Message);

                    var product = await productHttp.GetDataFromHttp(httpProductString);
                    if (product.StatusCode != 200)
                        return result.SendReponse(product.StatusCode, product.Message);
                    order.User = user.Data;
                    order.Product = product.Data;
                }

                return result.SendReponse(200, "Success", listOrder);
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async Task<AppReponse<OrderDTO>> Update(Order request)
        {
            var result = new AppReponse<OrderDTO>();
            try
            {
                return result.SendReponse(200, "Success");
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }
    }
}
