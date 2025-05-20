using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using UserService.Models.DTOs;
using UserService.Models.Enities;
using UserService.Models.Requests;
using UserService.Services.Implements;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var data = await _userService.ValidateRefreshToken(refreshToken);
            return Ok(data);
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var data = await _userService.Login(request);
            return Ok(data);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest request)
        {
            var data = await _userService.Search(request);
            return Ok(data);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = await _userService.Delete(Id);
            return Ok(data);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var data = await _userService.GetById(Id);
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _userService.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO request)
        {
            var data = await _userService.Create(request);
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User request)
        {
            var data = await _userService.Update(request);
            return Ok(data);
        }
    }
}
