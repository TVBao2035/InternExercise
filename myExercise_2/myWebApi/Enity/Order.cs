
namespace myWebApi.Enity
{
    public class Order
    {
        public Guid Id { get; set; }
    
        public Guid UserId { get; set; }
        public User? User { get; set; }

        
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
