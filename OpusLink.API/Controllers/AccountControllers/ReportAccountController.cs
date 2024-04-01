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
