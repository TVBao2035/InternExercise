using myWebApi.Data;
using myWebApi.Enity;
using myWebApi.Repository.GenericRepository;
using myWebApi.Repository.Interface;

namespace myWebApi.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
