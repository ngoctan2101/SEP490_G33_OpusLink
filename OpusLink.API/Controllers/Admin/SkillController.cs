using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.JobDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using System.IO;

namespace OpusLink.API.Controllers.Admin
{
    [Authorize(Roles = "Freelancer,Admin,Employer")]
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        readonly ISkillService _skillService;
        public IMapper _mapper;
        public SkillController(ISkillService skillService, IMapper mapper)
        {
            _skillService = skillService;
            _mapper = mapper;
        }
        [HttpPost("GetTenSkill")]
        public IActionResult GetTenSkill([FromBody] Filter filter)
        {
            int numberOfPage;
            List<Skill> skill = _skillService.GetAllSkill();
            var skillResultAfterSearch = Search(skill, filter, out numberOfPage);
            if (skill != null && skill.Count == 0)
            {
                return Ok("Don't have skill");
            }
            List<SkillDTO> result = _mapper.Map<List<SkillDTO>>(skillResultAfterSearch);
            result.Add(new SkillDTO { SkillID = numberOfPage });
            return Ok(result);
        }

        private object Search(List<Skill> skills, Filter filter, out int numberOfPage)
        {
            List<Skill> result = new List<Skill>();
            result = skills.ToList();
            //search string
            if (filter.SearchStr.Length > 0)
            {
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    if (result[i].SkillName.ToLower().Contains(filter.SearchStr.ToLower()))
                    {
                        continue;
                    }
                    else
                    {
                        result.RemoveAt(i);
                    }
                }
            }
            //loc theo page
            numberOfPage = result.Count / 10;
            if (result.Count % 10 > 0)
            {
                numberOfPage++;
            }
            return result.Skip((filter.PageNumber - 1) * 10).Take(10).ToList();
        }

        [HttpGet("GetAllSkill")]
        public IActionResult GetAllSkill()
        {

            List<Skill> skill = _skillService.GetAllSkill();
            if (skill != null && skill.Count == 0)
            {
                return Ok("Don't have skill");
            }
            return Ok(_mapper.Map<List<SkillDTO>>(skill));
        }
        [HttpGet("GetSkillById/{id}")]
        public IActionResult GetSkillById(int id)
        {

            Skill skill = _skillService.GetSkillById(id);
            if (skill == null)
            {
                return Ok("Don't have skill");
            }
            return Ok(_mapper.Map<SkillDTO>(skill));
        }

        [HttpGet("GetSkillsByUserId/{uid}")]
        public IActionResult GetSkillByUserId(int uid)
        {
            var skills = _skillService.GetAllSkillByUserID(uid);
            
            if (skills == null)
            {
                return NotFound("Don't have skill");
            }
            return Ok(skills);
        }

        [HttpPost("AddSkill")]
        public ActionResult<Skill> AddSkill([FromBody] SkillDTO skillDto)
        {

            try
            {
                List<Skill> skills = _skillService.GetAllSkill();
                var checkExist = _skillService.CheckExist(skills, skillDto.SkillID);
                if (checkExist)
                {
                    return BadRequest($"Skill with ID {skillDto.SkillID} exists in the list.");
                }
                _skillService.AddSkill(_mapper.Map<Skill>(skillDto));
                return Ok("Add succesfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateSkill")]
        public IActionResult Put([FromBody] SkillDTO skillDto)
        {
            try
            {
                if (skillDto == null)
                {
                    return BadRequest("Skill cannot be null");
                }
                _skillService.UpdateSkill(_mapper.Map<Skill>(skillDto));
                return Ok("Update succesfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteSkillById/{id}")]
        
        public IActionResult DeleteSkillById(int id)
        {

            try
            {
                _skillService.DeleteSkillById(id);
                return Ok("Delete successfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
