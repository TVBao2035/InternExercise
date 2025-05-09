using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myWebApi.Services;
using myWebApi.Services.Interface;

namespace myWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var data = await _orderDetailService.GetAll();
            return Ok(data);
        }
    }
}
