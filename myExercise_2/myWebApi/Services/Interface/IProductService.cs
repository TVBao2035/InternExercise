using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;

namespace myWebApi.Services.Interface
{
    public interface IProductService
    {
        Task<AppResponse<Product>> Create(ProductCreateRequest product);
        Task<AppResponse<Product>> Delete(Guid id);
        Task<AppResponse<List<Product>>> GetAll();
        Task<AppResponse<Product>> Update(Product product);
    }
}
