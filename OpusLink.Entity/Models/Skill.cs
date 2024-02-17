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
        public virtual ICollection<FreelancerWithSkill> FreelancerWithSkills { get; set; } = new List<FreelancerWithSkill>();
        public virtual Skill? SkillParent { get; set; }
    }
}
