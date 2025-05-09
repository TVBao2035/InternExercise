using myWebApi.Enity;

namespace myWebApi.Model.Request
{
    public class OrderCreateRequest
    {
        public Guid UserId { get; set; }
        public List<Product> Products { get; set; }
    }
}
