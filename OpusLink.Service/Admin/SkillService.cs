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
        Skill GetSkill(int skillId);
        void AddSkill(Skill skill);
        void UpdateSkill(Skill skill);
        void DeleteSkill(Skill skill);
    }
    public class SkillService : ISkillService
    {
        private readonly OpusLinkDBContext _context;
        public SkillService(OpusLinkDBContext context)
        {
            context = _context;
        }

        public void AddSkill(Skill skill)
        {
            try
            {
                

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteSkill(Skill skill)
        {
            throw new NotImplementedException();
        }

        public List<Skill> GetAllSkill()
        {
            try
            {
                var skill = _context.Skills.ToList();
                return skill;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Skill GetSkill(int skillId)
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
            throw new NotImplementedException();
        }
    }
}
