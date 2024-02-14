using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Service.Admin;

namespace OpusLink.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        private IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {

            var user = _userService.GetAllUser();
            if (user != null && user.Count == 0)
            {
                return Ok("Don't have user");
            }
            return Ok(_mapper.Map<UserDTO>(user));
        }
    }

    
}
