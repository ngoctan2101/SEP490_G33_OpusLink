using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int? CategoryParentID { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<JobInCategory> JobInCategories { get; set; } = new List<JobInCategory>();
        public virtual Category? CategoryParent { get; set; }
    }
}
