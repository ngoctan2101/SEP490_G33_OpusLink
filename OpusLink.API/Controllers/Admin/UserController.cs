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
        private ISkillService _skillService;
        public UserController(IUserService userService, IMapper mapper, ISkillService skillService)
        {
            _userService = userService;
            _mapper = mapper;
            _skillService = skillService;
        }
        [HttpGet("GetAllUser")]
        public IActionResult GetAllUser()
        {
            List<OpusLink.Entity.Models.User> users = _userService.GetAllUser();
            var userdto = _mapper.Map<List<UserDTO>>(users);
            if (users != null && users.Count == 0)
            {
                return NotFound("Don't have users");
            }
            foreach (var u in userdto)
            {
                List<string> lists = new List<string>();
                var skills = _skillService.GetAllSkillByUserID(u.Id);
                foreach (var s in skills)
                {
                    lists.Add(s.SkillName);
                }
                u.Skills = lists;
            }

            return Ok(userdto);
        }
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {

            var user = _userService.GetUserById(id);
            var userdto = _mapper.Map<UserDTO>(user);

            var skills = _skillService.GetAllSkillByUserID(id);
            List<string> lists = new List<string>();
            foreach (var s in skills)
            {
                lists.Add(s.SkillName);
            }
            userdto.Skills = lists;
            if (user == null)
            {
                return NotFound("Don't have user");
            }
            return Ok(userdto);
        }
        [HttpGet("GetUserByName/{txt}")]
        public IActionResult GetUserByName(string txt )
        {
            txt.Trim();
            var user = _userService.GetUserByName(txt);
            var userdto = _mapper.Map<List<UserDTO>>(user);
            if (user != null && user.Count == 0)
            {
                return NotFound("Don't have users");
            }
            
            foreach (var u in userdto)
            {
                List<string> lists = new List<string>();
                var skills = _skillService.GetAllSkillByUserID(u.Id);
                foreach (var s in skills)
                {
                    lists.Add(s.SkillName);
                }
                u.Skills = lists;
            }
            return Ok(userdto);
        }
    }

    
}
