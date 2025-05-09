using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;
using myWebApi.Repository;
using myWebApi.Repository.Interface;
using myWebApi.Services.Interface;

namespace myWebApi.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;

        }

        public async  Task<AppResponse<OrderDetail>> Create(OrderDetailCreateRequest order)
        {
            var response = new AppResponse<OrderDetail>();
            try
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Id = Guid.NewGuid();
                orderDetail.OrderId = order.OrderId;
                orderDetail.ProductId = order.ProductId;
                await _orderDetailRepository.Create(orderDetail);

                return response.Send(200, "success", orderDetail);
            }
            catch (Exception ex)
            {

                return response.Send(400, ex.Message);
            }
        }

        public Task<AppResponse<OrderDetail>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppResponse<List<OrderDetail>>> GetAll()
        {
            var response = new AppResponse<List<OrderDetail>>();
            try
            {
                var orderDetailList = await _orderDetailRepository.GetAll();
                return response.Send(200, "success", orderDetailList);
            }
            catch (Exception ex)
            {

                return response.Send(400, ex.Message);
            }
        }

        public Task<AppResponse<OrderDetail>> Update(OrderDetail order)
        {
            throw new NotImplementedException();
        }
    }
}
