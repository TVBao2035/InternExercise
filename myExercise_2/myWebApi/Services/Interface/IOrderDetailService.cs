using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;

namespace myWebApi.Services.Interface
{
    public interface IOrderDetailService
    {
        Task<AppResponse<OrderDetail>> Create(OrderDetailCreateRequest order);
        Task<AppResponse<OrderDetail>> Delete(Guid id);
        Task<AppResponse<List<OrderDetail>>> GetAll();
        Task<AppResponse<OrderDetail>> Update(OrderDetail order);
    }
}
