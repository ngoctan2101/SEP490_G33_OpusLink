using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
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
            List<OpusLink.Entity.Models.User> users = _userService.GetAllUser();
            if (users != null && users.Count == 0)
            {
                return Ok("Don't have users");
            }
            return Ok(_mapper.Map<List<UserDTO>>(users));
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {

            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return Ok("Don't have user");
            }
            return Ok(_mapper.Map<UserDTO>(user));
        }
        [HttpGet("CountUsers")]
        public IActionResult CountUsers()
        {
            int userCount = _userService.GetAllUser().Count;
            return Ok(userCount);
        }
    }

    
}
