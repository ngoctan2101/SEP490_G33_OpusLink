using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class GetCategoryResponse
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool HasChildCategory { get; set; }//hien thi nut tha xuong trong filter
        public int NumberOfJob { get; set; } //hien thi so luong job o moi category

        public int ParentId { get; set; } = 0;
        public string ParentName { get; set; } = "";
    }
}
