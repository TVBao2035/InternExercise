using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myWebApi.Model.Request;
using myWebApi.Services;
using myWebApi.Services.Interface;

namespace myWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _productService.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            var data = await _productService.Create(request);
            return Ok(data);
        }
    }
}
