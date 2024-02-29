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
        List<Skill> GetAllSkillByUserID(int id);
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
                if (skill.SkillParentID==null||skill.SkillParentID == 0)
                {
                    skill.SkillParentID = null;
                }
                skill.SkillParent = null;
                skill.FreelancerAndSkills = null;
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
        /*
        var products
    = from p in context.products
      join c in context.categories on p.CategoryId equals c.CategoryId
         */
        public List<Skill> GetAllSkillByUserID(int id)
        {
            try
            {
                //List<Skill> list = new List<Skill>();
                //var skills =( from s in _context.Skills
                //             join fs in _context.FreelancerAndSkills
                //             on s.SkillID equals fs.SkillID
                //             join us in _context.Users
                //             on fs.FreelancerID equals us.Id //ve tim user id
                //             where us.Id == id
                //             select new
                //             {
                //                 SkillID = s.SkillID,
                //                 SkillParentID = s.SkillParentID,
                //                 SkillName = s.SkillName
                                 
                //             }).Distinct();
                ////join u in _context.Users
                //foreach (var item in skills)
                //{
                    
                //        var skill = new Skill
                //        {
                //            SkillID = item.SkillID,
                //            SkillParentID = item.SkillParentID,
                //            SkillName = item.SkillName
                //        };
                //        list.Add(skill);
                    
                //}
                var freelancerAndSkills =_context.FreelancerAndSkills.Where(fs=>fs.FreelancerID== id).Include("Skill").ToList();
                List<Skill> list = new List<Skill>();
                foreach(var fs in freelancerAndSkills)
                {
                    list.Add(fs.Skill);
                }
                return list;

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
                if (skill.SkillParentID==null||skill.SkillParentID == 0)
                {
                    skill.SkillParentID = null;
                    skill.SkillParent = null;
                }
                skill.FreelancerAndSkills = null;
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
