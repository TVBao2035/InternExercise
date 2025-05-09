using myWebApi.Data;
using myWebApi.Enity;
using myWebApi.Repository.GenericRepository;
using myWebApi.Repository.Interface;

namespace myWebApi.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
