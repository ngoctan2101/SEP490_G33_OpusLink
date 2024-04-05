using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO.AccountDTO.Common;
using OpusLink.Entity.DTO.ReportUserDTO;

namespace OpusLink.API.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly OpusLinkDBContext _context;
        private readonly IMapper _mapper;

        public ReportController(OpusLinkDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        [HttpGet("GetAllReportById")]
        public async Task<IActionResult> GetAllReportById(int id)
        {
            var reports = await _context.ReportUsers
                .Include(r => r.CreateByUser)
                .Where(r => r.TargetToUserID == id)
                .ToListAsync();

            if (reports == null || reports.Count == 0)
            {
                return NotFound("No reports found for the given user ID.");
            }

            var reportInfoDTOs = new List<ReportInfoDTO>();

            foreach (var report in reports)
            {
                var reportInfoDTO = _mapper.Map<ReportInfoDTO>(report);
                reportInfoDTO.DateCreated = report.DateCreated.ToString("dd/MM/yyyy");
                reportInfoDTOs.Add(reportInfoDTO);
            }

            return Ok(reportInfoDTOs);
        }
    }
}
