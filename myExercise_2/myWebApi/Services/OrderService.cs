using Microsoft.EntityFrameworkCore;
using myWebApi.Data;
using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;
using myWebApi.Repository;
using myWebApi.Repository.Interface;
using myWebApi.Services.Interface;

namespace myWebApi.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IOrderDetailService _orderDetailService;
        private ApplicationDbContext _context;

        public OrderService(IOrderRepository orderRepository, IOrderDetailService orderDetailService, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _orderDetailService = orderDetailService;
            _context = context;
        }

        public async  Task<AppResponse<Order>> Create(OrderCreateRequest order)
        {
            var response = new AppResponse<Order>();
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                
                    Order ord = new Order();
                    ord.Id = Guid.NewGuid();
                    ord.UserId = order.UserId;
                    await _orderRepository.Create(ord);
                    OrderDetailCreateRequest orderDetailCreateRequest = new OrderDetailCreateRequest();
                    foreach (var product in order.Products)
                    {
                        orderDetailCreateRequest.OrderId = ord.Id;
                        orderDetailCreateRequest.ProductId = product.Id;
                        await _orderDetailService.Create(orderDetailCreateRequest);
                    }
                    transaction.Commit();
                    return response.Send(200, "success");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return response.Send(400, ex.Message);
                }

            }
        }

        public Task<AppResponse<Order>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppResponse<List<Order>>> GetAll()
        {
            var response = new AppResponse<List<Order>>();
            try
            {
                var orderList = await _orderRepository.GetAll();

                return response.Send(200, "success", orderList);
            }
            catch (Exception ex)
            {

                return response.Send(400, ex.Message);
            }
        }

        public async Task<AppResponse<Order>> Update(Order order)
        {
            var response = new AppResponse<Order>();
            try
            {


                return response.Send(200, "success");
            }
            catch (Exception ex)
            {

                return response.Send(400, ex.Message);
            }
        }
    }
}
