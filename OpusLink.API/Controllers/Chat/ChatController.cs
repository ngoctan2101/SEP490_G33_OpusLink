using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using OpusLink.Service.Admin;
using OpusLink.Service.Chat;
using System;
using System.IO;

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
        public ActionResult<IEnumerable<ChatDTO>> GetAllChat()
        {
            List<ChatBox> chat = _chatService.getAllChatBox()
                .ToList();
            List<ChatDTO> list = _mapper.Map<List<ChatDTO>>(chat);
            return Ok(list);
        }
        [HttpGet("GetChatBoxById/{id}")]
        public IActionResult GetChatBoxById(int id)
        {
            var chat = _chatService.getChatBoxById(id);
            if (chat == null)
            {
                return Ok("Don't have chat");
            }
            return Ok(_mapper.Map<ChatDTO>(chat));






        }
        [HttpGet("GetMessageById/{id}")]
        public IActionResult GetMessageById(int id)
        {
            var message = _chatService.GetMessageById(id);
            if (message == null)
            {
                return Ok("Don't have message");
            }
            return Ok(_mapper.Map<List<MessageDTO>>(message));


        }
    }
}

