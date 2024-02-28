using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class Filter
    {
        public List<int> CategoryIDs { get; set; } = new List<int>();
        public List<int> Statuses { get; set; } = new List<int>();
        public string SearchStr { get; set; }
        public decimal BudgetMin { get; set; }
        public decimal BudgetMax { get; set; }
        public DateTime DateMin { get; set; }
        public DateTime DateMax { get; set; }
        public int PageNumber { get; set; }
        public int UserId { get; set; }
        public string getDateRange()
        {
            return DateMin.ToString("MM/dd/yyyy") + " - " + DateMax.ToString("MM/dd/yyyy");
        }
        public List<string> GetAllCategoryName(IList<GetCategoryResponse> Categories)
        {
            List<string> result = new List<string>();
            foreach (var id in CategoryIDs)
            {
                result.Add(Categories.Where(x => x.CategoryID == id).First().CategoryName);
            }
            return result;
        }
        public List<int> GetAllNumberOfJob(IList<GetCategoryResponse> Categories)
        {
            List<int> result = new List<int>();
            foreach (var id in CategoryIDs)
            {
                result.Add(Categories.Where(x => x.CategoryID == id).First().NumberOfJob);
            }
            return result;
        }
    }
}
