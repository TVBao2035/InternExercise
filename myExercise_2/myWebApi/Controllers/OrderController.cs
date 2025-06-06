﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myWebApi.Model.Request;
using myWebApi.Services;
using myWebApi.Services.Interface;

namespace myWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService) {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _orderService.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest request)
        {
            var data = await _orderService.Create(request);
            return Ok(data);
        }
    }
}
