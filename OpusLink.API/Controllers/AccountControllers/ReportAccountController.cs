using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity;
using OpusLink.Entity.DTO.AccountDTO;
using OpusLink.Entity.DTO.ReportUserDTO;
using OpusLink.Entity.Models;

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
        public IActionResult AddNewReport(ReportAccountDTO reportUser)
        {
            ReportUser reportAccount = new ReportUser()
            {
                CreateByUserID = reportUser.CreateByUserID,
                TargetToUserID = reportUser.TargetToUserID,
                ReportUserContent = reportUser.ReportUserContent,
                DateCreated = DateTime.Now
            };
            _context.ReportUsers.Add(reportAccount);
            _context.SaveChanges();
            return Ok("Add report successfull");
        }
    }
}
