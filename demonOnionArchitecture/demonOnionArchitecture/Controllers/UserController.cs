using demonOnionArchitecture.Domain.DTOs.Request;
using demonOnionArchitecture.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace demonOnionArchitecture.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService )
        {
            _userService = userService;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UserUpdateRequest request)
        {
            var data = await _userService.Update(request);
            return Ok(data);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var data = await _userService.Delete(Id);
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var  data = await _userService.GetAll();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            var data = await _userService.Create(request);
            return Ok(data);
        }
    }
}
