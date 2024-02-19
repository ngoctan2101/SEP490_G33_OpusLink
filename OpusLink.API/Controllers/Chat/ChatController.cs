using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private IMapper _mapper;
        public ChatController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }
        [HttpGet("GetAllChat")]
        public IActionResult GetAllChat()
        {
            var chat = _chatService.getAllChatBox();
            if (chat != null && chat.Count == 0)
            {
                return Ok("Don't have chat");
            }
            return Ok(_mapper.Map<ChatBox>(chat));
        }
    }
}
