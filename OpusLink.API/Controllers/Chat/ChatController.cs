﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OpusLink.API.Hubs;
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
        readonly IHubContext<ChatHub> _hubContext;
        public ChatController(IHubContext<ChatHub> hubContext,IChatService chatService, IMapper mapper)
        {
            _hubContext = hubContext;
            _chatService = chatService;
            _mapper = mapper;
        }
        [HttpGet("GetAllChat/{userId}/{role}")]
        public ActionResult<IEnumerable<ChatDTO>> GetAllChat(int userId, int role)
        {
            List<ChatBox> chat = _chatService.getAllChatBox(userId, role)
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


        [HttpPost("CreateMessage")]
        public IActionResult CreateMessage([FromBody] CreateMessageDTO createMessageDTO)
        {
            try
            {
                // Create the message
                var createdMessage = _chatService.CreateMessage(createMessageDTO);

                if (createdMessage == null)
                {
                    return NotFound("Failed to create message");
                }

                // Broadcast the message to clients
                _hubContext.Clients.All.SendAsync("ReceiveMessage", createdMessage);

                return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.MessageID }, createdMessage);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update errors
                return StatusCode(500, $"Database error: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("CreateChatBox")]
        public IActionResult CreateChatBox([FromBody] CreateChatBoxDTO createChatBoxDTO)
        {
            try
            {
                var createdChatBox = _chatService.CreateChatBox(createChatBoxDTO);
                return Ok(createdChatBox);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

