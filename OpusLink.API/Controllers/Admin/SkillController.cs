using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Service.Admin;

namespace OpusLink.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        readonly ISkillService _skillService;
        private IMapper _mapper;
        public SkillController(ISkillService skillService, IMapper mapper)
        {
            _skillService = skillService;
            _mapper = mapper;
        }
        [HttpGet("GetAllSkill")]
        public IActionResult GetAllSkill()
        {

            var skill = _skillService.GetAllSkill();
            if (skill != null && skill.Count == 0)
            {
                return Ok("Don't have skill");
            }
            return Ok(_mapper.Map<SkillDTO>(skill));
        }
    }
}
