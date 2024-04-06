using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Service.Admin;

namespace OpusLink.API.Controllers.UserControllers.Freelance
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        readonly IUserService _userService;
        private IMapper _mapper;
        private ISkillService _skillService;
        public ProfileController(IUserService userService, IMapper mapper, ISkillService skillService)
        {
            _userService = userService;
            _mapper = mapper;
            _skillService = skillService;
        }
        //[HttpGet("GetAllUser")]
        //public IActionResult GetAllUser()
        //{
        //    List<OpusLink.Entity.Models.User> users = _userService.GetAllUser();
        //    if (users != null && users.Count == 0)
        //    {
        //        return Ok("Don't have users");
        //    }
        //    return Ok(_mapper.Map<List<UserDTO>>(users));
        //}
        //[HttpGet("GetUserById/{id}")]
        //public IActionResult GetUserById(int id)
        //{

        //    var user = _userService.GetUserById(id);
        //    var userdto = _mapper.Map<UserDTO>(user);

        //    var skills = _skillService.GetAllSkillByUserID(id);
        //    List<string> lists = new List<string>();
        //    foreach (var s in skills)
        //    {
        //        lists.Add(s.SkillName);
        //    }
        //    userdto.Skills = lists;
        //    if (user == null)
        //    {
        //        return NotFound("Don't have user");
        //    }
        //    return Ok(userdto);
        //}
    }
}
