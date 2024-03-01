using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO
{
    public class SkillDTO
    {
        public int SkillID { get; set; }
        public int? SkillParentID { get; set; }
        public string SkillName { get; set; }
        public string? SkillParentName { get; set; }
    }
}
