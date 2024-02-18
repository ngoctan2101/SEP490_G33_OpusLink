using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.Models.JOB
{
    public class GetJobResponse
    {
        public int JobID { get; set; } = 0;
        public string? JobTitle { get; set; }
        public int EmployerID { get; set; } = 0;
        public string EmployerName { get; set; } = "";
        public string EmployerStarMedium { get; set; } = "";
        public int? FreelancerID { get; set; }
        public string? JobContent { get; set; }
        public decimal? BudgetFrom { get; set; }
        public decimal? BudgetTo { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? LocationID { get; set; }
        public int Status { get; set; } = 0;
        public List<GetCategoryResponse> Categoies { get; set; }= new List<GetCategoryResponse>();
        public int NumberOfOffer { get; set; } = 0;

        //Will add more, because this class used in many usecases

    }
}
