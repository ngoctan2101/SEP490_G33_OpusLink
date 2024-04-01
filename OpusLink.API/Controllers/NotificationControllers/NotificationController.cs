using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity.DTO;
using OpusLink.Entity.DTO.NotificationDTO;
using OpusLink.Entity.Models;
using OpusLink.Service.NotificationServices;
using OpusLink.Service.PaymentServices;

namespace OpusLink.API.Controllers.NotificationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        readonly INotificationServices _iNotificationServices;
        public IMapper _mapper;
        public NotificationController(INotificationServices iNotificationServices, IMapper mapper)
        {
            _iNotificationServices = iNotificationServices;
            _mapper = mapper;
        }

        [HttpGet("Get5NotificationNew/{uid}")]
        public ActionResult<IEnumerable<NotificationDTO>> Get5NotificationNew(int uid)
        {
            try
            {
                return Ok(_mapper.Map<List<NotificationDTO>>(_iNotificationServices.Get5NotificationNew(uid)));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpGet("GetAllNotification/{uid}")]
        public ActionResult<IEnumerable<NotificationDTO>> GetAllNotification(int uid)
        {
            try
            {
                return Ok(_mapper.Map<List<NotificationDTO>>(_iNotificationServices.GetAllNotification(uid)));
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpPost("AddNotification")]
        public ActionResult<NotificationDTO> AddNotification([FromBody] NotificationDTO notiDto)
        {

            try
            {
                var noti = _mapper.Map<Notification>(notiDto);

                _iNotificationServices.AddHNotification(noti);

                return Ok("Add thanh cong");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateNotificationReader/{notiId}")]
        public IActionResult UpdateNotificationReader(int notiId)
        {
            try
            {
                return Ok(_mapper.Map<NotificationDTO>(_iNotificationServices.UpdateNotificationReader(notiId)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("CountNotificationNew/{uid}")]
        public IActionResult CountNotificationNew(int uid)
        {
            try
            {
                int count = _iNotificationServices.CountNotificationNew(uid);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
