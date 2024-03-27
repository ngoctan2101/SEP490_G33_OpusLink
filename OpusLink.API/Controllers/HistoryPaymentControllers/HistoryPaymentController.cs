using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using OpusLink.Service.PaymentServices;

namespace OpusLink.API.Controllers.HistoryPaymentControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryPaymentController : ControllerBase
    {
        readonly IHistoryPaymentService _historyPaymentService;
        public IMapper _mapper;
        public HistoryPaymentController(IHistoryPaymentService historyPaymentService, IMapper mapper)
        {
            _historyPaymentService = historyPaymentService;
            _mapper = mapper;
        }

        [HttpGet("GetAllHistoryPayment")]
        public ActionResult<IEnumerable<HistoryPaymentDTO>> GetAllHistoryPayment()
        {
            try
            {
                return Ok(_mapper.Map<List<HistoryPaymentDTO>>(_historyPaymentService.GetAllHistoryPayment()));
            }catch (Exception ex)
            {
                return NotFound();
            }
        }

        //[HttpGet("GetDetailHistoryPaymentByUserId/{id}")]
        //public IActionResult GetDetailHistoryPaymentByUserId(int id)
        //{
        //    var histories = _historyPaymentService.getHistoryPaymentsByUserId(id);
        //    if (histories == null)
        //    {
        //        return Ok("Don't have HistoryPayment");
        //    }
        //    return Ok(_mapper.Map<List<HistoryPaymentDTO>>(histories));
        //}

        [HttpGet("GetHistoryPaymentById/{id}")]
        public IActionResult GetHistoryPaymentId(int id)
        {

            HistoryPayment his = _historyPaymentService.GetHistoryPaymentById(id);
            if (his == null)
            {
                return Ok("Don't have HistoryPayment");
            }
            return Ok(_mapper.Map<HistoryPaymentDTO>(his));
        }

        [HttpGet("GetHistoryPaymentByUserId/{id}")]
        public IActionResult GetHistoryPaymentByUserId(int id)
        {

            var his = _historyPaymentService.GetHistoryPaymentByUserId(id);
            if (his == null)
            {
                return Ok("Don't have HistoryPayment");
            }
            return Ok(_mapper.Map<List<HistoryPaymentDTO>>(his));
        }
        [HttpPost("AddHistoryPayment")]
        public ActionResult<HistoryPayment> AddHistoryPayment([FromBody] HistoryPaymentDTO hisDto)
        {

            try
            {
                List<HistoryPayment> history = _historyPaymentService.GetAllHistoryPayment();
                var checkExist = _historyPaymentService.CheckExist(history, hisDto.PaymentID);
                if (checkExist)
                {
                    return BadRequest($"HistoryPayment with ID {hisDto.PaymentID} exists in the list.");
                }

               
                return Ok(_historyPaymentService.AddHistoryPayment(_mapper.Map<HistoryPayment>(hisDto)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateHistoryPayment")]
        public IActionResult UpdateHistoryPayment([FromBody] HistoryPaymentDTO hisDto)
        {
            try
            {
                if (hisDto == null)
                {
                    return BadRequest("HistoryPayment cannot be null");
                }
                _historyPaymentService.UpdateHistoryPayment(_mapper.Map<HistoryPayment>(hisDto));
                return Ok("Update succesfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteHistoryPayment/{id}")]

        public IActionResult DeleteHistoryPayment(int id)
        {

            try
            {
                _historyPaymentService.DeleteHistoryPayment(id);
                return Ok("Delete successfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
