using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Service.Admin;
using OpusLink.Service.PaymentServices;
using System.Collections.Generic;

namespace OpusLink.API.Controllers.Admin
{
	[Route("api/[controller]")]
	[ApiController]
	public class InComeController : Controller
	{
		readonly IHistoryPaymentService _historyPaymentService;
		public IMapper _mapper;
		public InComeController(IHistoryPaymentService historyPaymentService, IMapper mapper)
		{
			_historyPaymentService = historyPaymentService;
			_mapper = mapper;
		}

		[HttpGet("GetDataIncome/{year}")]
		public async Task<ActionResult<DataIncomePerYear>> GetDataIncomeAsync([FromRoute] int year)
		{
			try
			{
				return Ok(await _historyPaymentService.GetDataIncome(year));
			}
			catch (Exception ex)
			{
				return NotFound();
			}
		}
        [HttpGet("GetHistory/{month}/{year}")]
        public async Task<ActionResult<List<HistoryPaymentDTO>>> GetHistory([FromRoute] int month, [FromRoute] int year)
        {
            try
            {
                return Ok(_mapper.Map<List<HistoryPaymentDTO>>(await _historyPaymentService.GetHistory(month, year)));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
