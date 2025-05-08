using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myWebApi.Enity;
using myWebApi.Services;

namespace myWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService) {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]User user)
        {
            var data = await _userService.Create(user);
            if (data is not null) return Ok(data);
            else return BadRequest(data);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]User user)
        {
            var data = await _userService.Update(user);
            if (data is not null) return Ok(data);
            return BadRequest(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _userService.GetAll();
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public  async Task<IActionResult> Delete(Guid id)
        {
            var data = await _userService.Delete(id);
            if(data is null) return BadRequest(data);
            else return Ok(data);
        }
    }
}
