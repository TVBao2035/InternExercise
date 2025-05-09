using myWebApi.Data;
using myWebApi.Enity;
using myWebApi.Repository.GenericRepository;
using myWebApi.Repository.Interface;

namespace myWebApi.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
