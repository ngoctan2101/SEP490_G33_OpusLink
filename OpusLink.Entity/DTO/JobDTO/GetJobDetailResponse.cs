using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.JobDTO
{
    public class GetJobDetailResponse
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; }
        public List<GetCategoryResponse> Categories { get; set; }=new List<GetCategoryResponse>();
        public string JobContent { get; set; }
        public int Status { get; set; }
        public int EmployerId { get; set; }
        public int? FreelancerId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerImagePath { get; set; }
        public byte[] EmployerImageBytes { get; set; }
        public int EmployerStarMedium { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal BudgetMin { get; set; }
        public decimal BudgetMax { get; set; }
        public string Location { get; set; }
        public int LocationId { get; set; }
        public int TotalOffer { get; set; }
        public decimal AverageCost { get; set; }
        public int AverageTime { get; set; }

    }
}
