using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.WithdrawRequestDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using OpusLink.Service.UserServices;
using OpusLink.Service.WithDrawRequestServices;

namespace OpusLink.API.Controllers.WithDrawRequestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WithDrawRequestController : ControllerBase
    {
        readonly IWithDrawRequestService _withService;
        private IMapper _mapper;
       
        public WithDrawRequestController(IWithDrawRequestService withService, IMapper mapper)
        {
            _withService = withService;
            _mapper = mapper;
           
        }
        [HttpGet("GetAllWithdrawRequestByStatus/{status}")]
        public IActionResult GetAllWithdrawRequestByStatus(int status)
        {
           
            var wdr =  _withService.GetAllWithdrawRequestByStatus(status);

            if(wdr == null)
            {
                return BadRequest("Status is limited from 1 to 3 ");
            }
            return Ok(_mapper.Map<List<WithdrawResponseDTO>>(wdr));
        }

        [HttpGet("GetAllWithdrawRequestById/{wid}")]
        public IActionResult GetAllWithdrawRequestById(int wid)
        {

            var wdr = _withService.GetAllWithdrawRequestById(wid);
            if (wdr == null)
            {
                return BadRequest("Not find id ");
            }
            return Ok(_mapper.Map<WithdrawResponseDTO>(wdr));
        }
        [HttpPost("AddWithdrawRequest")]
        public IActionResult AddWithdrawRequest([FromBody] WithdrawRequestDTO with)
        {

            try
            {
                //var respon = _mapper.Map<WithdrawRequest>(with);

                //_withService.AddWithdrawRequest(with);
                _withService.AddWithdrawRequest(_mapper.Map<WithdrawRequest>(with));

                return Ok("Add cussefful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateHisIdWithdrawRequest/{wId}/{hisId}")]
        public IActionResult UpdateHisIdWithdrawRequest(int wId, int hisId)
        {
            try
            {
                _withService.UpdateHisIdWithdrawRequest(wId, hisId);
                return Ok("Update successfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateHWithdrawRequestByStatusToSuccessfull/{wid}")]
        public IActionResult UpdateHWithdrawRequestByStatusToSuccessfull(int wid)
        {
            try
            {
                var result = _withService.UpdateHWithdrawRequestByStatusToSuccessfull(wid);
                if(result == false) { 
                    return StatusCode(500); 
                }
                return Ok("Update succesfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateHWithdrawRequestByStatusToFail/{wid}/{reason}")]
        public IActionResult UpdateHWithdrawRequestByStatusToFail(int wid, string reason)
        {
            reason.Trim();
            try
            {
                var result = _withService.UpdateHWithdrawRequestByStatusToFail(wid, reason);
                if (result == false)
                {
                    return StatusCode(500);
                }
                return Ok("Update succesfull");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
