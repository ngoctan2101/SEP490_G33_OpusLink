using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.Models;
using OpusLink.Entity;
using OpusLink.Entity.DTO.AccountDTO;
using Microsoft.AspNetCore.Authorization;

namespace OpusLink.API.Controllers.AccountControllers
{
    //[Authorize(Roles = "Freelancer,Employer")]
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
        public IActionResult AddNewReport(ReportAccountDTO reportAccountDTO)
        {
            var job = _context.Jobs.FirstOrDefault(j => j.EmployerID == reportAccountDTO.CreateByUserID && j.FreelancerID == reportAccountDTO.TargetToUserID ||
            j.EmployerID == reportAccountDTO.TargetToUserID && j.FreelancerID == reportAccountDTO.CreateByUserID);
            if (job == null)
            {
                return BadRequest("Bạn không được quyền Report tài khoản này");
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
                return Ok("Thành công");
            }
        }
    }
}
