using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models.JOB
{
    public class GetCategoryResponse
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool HasChildCategory { get; set; }//hien thi nut tha xuong trong filter
        public int NumberOfJob { get; set; } //hien thi so luong job o moi category
    }
}
