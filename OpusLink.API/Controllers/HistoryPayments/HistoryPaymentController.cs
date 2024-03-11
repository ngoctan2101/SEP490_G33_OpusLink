using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Chat;
using OpusLink.Service.HistoryPaymentService;

namespace OpusLink.API.Controllers.HistoryPayments
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryPaymentController: ControllerBase
    {
        readonly IHistoryPaymentService _historyPaymentService;
        readonly IMapper _mapper;
        public HistoryPaymentController(IHistoryPaymentService historyPaymentService, IMapper mapper)
        {
            _historyPaymentService = historyPaymentService;
            _mapper = mapper;
        }

        [HttpGet("GetHistoryPayment")]
        public ActionResult<IEnumerable<HistoryPaymentDTO>> GetHistoryPayment()
        {
            List<HistoryPayment> histories = _historyPaymentService.getHistoryPayments()
                .ToList();
            List<HistoryPaymentDTO> list = _mapper.Map<List<HistoryPaymentDTO>>(histories);
            return Ok(list);
        }

        [HttpGet("GetHistoryPaymentByUserId/{id}")]
        public IActionResult GetHistoryPaymentByUserId(int id)
        {
            var histories = _historyPaymentService.getHistoryPaymentsByUserId(id);
            if (histories == null)
            {
                return Ok("Don't have HistoryPayment");
            }
            return Ok(_mapper.Map<List<HistoryPaymentDTO>>(histories));
        }
    }
   
}
