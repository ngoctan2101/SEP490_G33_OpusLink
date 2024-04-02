using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.Models;
using OpusLink.Entity;
using OpusLink.Entity.DTO.AccountDTO;

namespace OpusLink.API.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportAccountController : ControllerBase
    {
        private readonly OpusLinkDBContext _context;

        public ReportAccountController(OpusLinkDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ApiResponseModel> AddNewReport(ReportAccountDTO reportAccountDTO)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.EmployerID == reportAccountDTO.CreateByUserID && j.FreelancerID == reportAccountDTO.TargetToUserID ||
            j.EmployerID == reportAccountDTO.TargetToUserID && j.FreelancerID == reportAccountDTO.CreateByUserID);

            if (job == null)
            {
                return new ApiResponseModel
                {
                    Code = 400,
                    Message = "Bạn không được quyền Report tài khoản này",
                    IsSuccess = false
                };
            }
            else
            {
                ReportUser reportAccount = new ReportUser()
                {
                    CreateByUserID = reportAccountDTO.CreateByUserID,
                    TargetToUserID = reportAccountDTO.TargetToUserID,
                    ReportUserContent = reportAccountDTO.ReportUserContent,
                    DateCreated = DateTime.Now
                };
                _context.ReportUsers.Add(reportAccount);
                int number = _context.SaveChanges();
                return new ApiResponseModel
                {
                    Code = 200,
                    Message = "Success",
                    IsSuccess = true
                };
            }
        }
    }
}
