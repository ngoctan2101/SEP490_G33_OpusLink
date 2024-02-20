using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpusLink.Entity;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using OpusLink.Service.Chat;

namespace OpusLink.API.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChatController : ControllerBase
    {
        readonly IChatService _chatService;
         readonly IMapper _mapper;
        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }
        [HttpGet("GetAllChat")]
        public ActionResult <IEnumerable<ChatDTO>> GetAllChat()
        {
            var chat = _chatService.getAllChatBox()
                .ToList().AsQueryable();
            List<ChatDTO> list = _mapper.Map<List<ChatDTO>>(chat);
            return Ok(list);
        }
    }
}
