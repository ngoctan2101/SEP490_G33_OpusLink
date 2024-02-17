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
        public virtual ICollection<JobAndCategory> JobAndCategories { get; set; } = new List<JobAndCategory>();
        public virtual Category? CategoryParent { get; set; }
    }
}
