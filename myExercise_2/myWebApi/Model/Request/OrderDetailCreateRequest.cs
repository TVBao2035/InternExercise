
using myWebApi.Enity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace myWebApi.Model.Request
{
    public class OrderDetailCreateRequest
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
    }
}
