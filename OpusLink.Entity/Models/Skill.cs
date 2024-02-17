using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Skill
    {
        public int SkillID { get; set; }
        public int? SkillParentID { get; set; }
        public string SkillName { get; set; }
        public virtual ICollection<FreelancerAndSkill> FreelancerAndSkills { get; set; } = new List<FreelancerAndSkill>();
        public virtual Skill? SkillParent { get; set; }
    }
}
