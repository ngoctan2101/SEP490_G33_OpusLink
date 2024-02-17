using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class FreelancerAndSkill
    {
        public int FreelancerAndSkillID { get; set; }
        public int FreelancerID { get; set; }
        public int SkillID { get; set; }
        public virtual Skill? Skill { get; set; }
        public virtual User? Freelancer { get; set; }
    }
}
