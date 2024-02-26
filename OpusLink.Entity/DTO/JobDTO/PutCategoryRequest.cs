using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class PutCategoryRequest
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentID { get; set; }
    }
}
