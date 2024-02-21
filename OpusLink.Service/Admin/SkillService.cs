using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.Admin
{
    public interface ISkillService
    {
        List<Skill> GetAllSkill();
        Skill GetSkillById(int skillId);
        void AddSkill(Skill skill);
        void UpdateSkill(Skill skill);
        void DeleteSkillById(int idSkill);
        bool CheckExist(List<Skill> skillList, int targetSkillId);
    }
    public class SkillService : ISkillService
    {
        public readonly OpusLinkDBContext _context = new OpusLinkDBContext();
        public SkillService()
        {
            
        }

        public void AddSkill(Skill skill)
        {
            try
            {
                _context.Skills.Add(skill);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception("Error adding skill", e);
            }
        }

        public void DeleteSkillById(int idSkill)
        {
            try
            {
                var skill = _context.Skills.FirstOrDefault(x=>x.SkillID == idSkill);
                var freeandskill = _context.FreelancerAndSkills.ToList();
                foreach(var item in freeandskill)
                {
                    if(item.SkillID == idSkill)
                    {
                        _context.FreelancerAndSkills.RemoveRange(item);
                    }
                }
                _context.Skills.Remove(skill);
                _context.SaveChanges();
                

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Skill> GetAllSkill()
        {
            try
            {
                var skill = _context.Skills.Include(x=>x.SkillParent).ToList();
                return skill;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Skill GetSkillById(int skillId)
        {
            try
            {
                var skill = _context.Skills.FirstOrDefault(x=>x.SkillID == skillId);
                return skill;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateSkill(Skill skill)
        {
            try
            {
              
                _context.Entry(skill).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public bool CheckExist(List<Skill> skillList, int targetSkillId)
        {
            return skillList.Any(skill => skill.SkillID == targetSkillId);
        }
    }
}
