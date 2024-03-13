using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO.AccountDTO.Common;

namespace OpusLink.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly OpusLinkDBContext _context;

        public ReportController(OpusLinkDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.ReportUsers.Include(x => x.CreateByUser).ToListAsync();
            if (result == null || result.Count == 0)
            {
                return NotFound("Don't have report");
            }
            else
            {
                return Ok(result);
            }
        }

    }
}
