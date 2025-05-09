using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;

namespace myWebApi.Services.Interface
{
    public interface IOrderService
    {
        Task<AppResponse<Order>> Create(OrderCreateRequest order);
        Task<AppResponse<Order>> Delete(Guid id);
        Task<AppResponse<List<Order>>> GetAll();
        Task<AppResponse<Order>> Update(Order order);
    }
}
