using AutoMapper;
using Azure;
using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;
using myWebApi.Repository;
using myWebApi.Repository.Interface;
using myWebApi.Services.Interface;

namespace myWebApi.Services
{
    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async  Task<AppResponse<Product>> Create(ProductCreateRequest product)
        {
            var response = new AppResponse<Product>();
            try
            {
                Product prod = new Product();
                prod.Id = Guid.NewGuid();
                prod = _mapper.Map<Product>(product);
                await _productRepository.Create(prod);  
                return response.Send(200, "Create Success", prod);
            }
            catch (Exception ex)
            {

                return response.Send(400, ex.Message);
            }
        }

        public Task<AppResponse<Product>> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppResponse<List<Product>>> GetAll()
        {
            var response = new AppResponse<List<Product>>();
            try
            {
                var productList = await _productRepository.GetAll();
                return response.Send(200, "Get All Product Success", productList);
            }
            catch (Exception ex)
            {

                return response.Send(400, ex.Message);
            }
        }

        public Task<AppResponse<Product>> Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
