using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class JobInCategory
    {
        public int JobInCategoryID { get; set; }
        public int CategoryID { get; set; }
        public int JobID { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Job? Job { get; set; }
    }
}
