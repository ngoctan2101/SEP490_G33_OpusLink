﻿using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
using OpusLink.Entity.DTO;
using OpusLink.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Service.Chat
{
    public interface IChatService
    {
        List<ChatBox> getAllChatBox();
        List<ChatBox> getChatBoxesByUserIdAndRole(int userId, string role);
        ChatBox getChatBoxById(int id);
        ChatBox getChatBoxByUserId(int id);
        List<Message> GetMessageById(int id);
        MessageDTO CreateMessage(CreateMessageDTO createMessageDTO);
        ChatDTO CreateChatBox(CreateChatBoxDTO createChatBoxDTO);
    }
    public class ChatService : IChatService
    {
        private readonly OpusLinkDBContext _context;
        public ChatService(OpusLinkDBContext context)
        {
            _context = context;
        }
        public List<ChatBox> getAllChatBox()
        {
            try
            {
                var chatBox = _context.ChatBoxs.Include("Freelancer").Include("Employer").Include("Messages").ToList();
                return chatBox;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<ChatBox> getChatBoxesByUserIdAndRole(int userId, string role)
        {
            try
            {
                var chatBoxes = _context.ChatBoxs.Include(cb => cb.Messages).Include("Freelancer").Include("Employer")
					.Where(cb => cb.EmployerID == userId || cb.FreelancerID == userId)

                    .ToList();
                return chatBoxes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public ChatBox getChatBoxById(int id)
        {
            try
            {
                var chatBox = _context.ChatBoxs.Include("Freelancer").Include("Employer").Include("Messages").FirstOrDefault(x => x.ChatBoxID == id);
                return chatBox;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public ChatBox getChatBoxByUserId(int id)
        {
            try
            {
                var chatBox = _context.ChatBoxs.Include("Freelancer").Include("Employer").Include("Messages").FirstOrDefault(x => x.FreelancerID == id || x.EmployerID == id);
                return chatBox;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Message> GetMessageById(int id)
        {
            try
            {
                var message = _context.Messages.Include("ChatBox").Include("ChatBox.Employer").Include("ChatBox.Freelancer").Where(x => x.ChatBoxID == id).ToList();
                return message;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public MessageDTO CreateMessage(CreateMessageDTO createMessageDTO)
        {
            var message = new Message
            {
                ChatBoxID = createMessageDTO.ChatBoxID,
                FromEmployer = createMessageDTO.FromEmployer,
                DateCreated = DateTime.Now,
                MessageContent = createMessageDTO.MessageContent
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            return new MessageDTO
            {
                MessageID = message.MessageID,
                ChatBoxID = message.ChatBoxID,
                FromEmployer = message.FromEmployer,
                DateCreated = message.DateCreated,
                MessageContent = message.MessageContent
            };
        }
        public Boolean isChatBoxExisted ( ChatBox chatBox)
        {
            return _context.ChatBoxs.Any(x => x.EmployerID == chatBox.EmployerID &&
            x.FreelancerID == chatBox.FreelancerID && x.JobID == chatBox.JobID);
        }
        public ChatDTO CreateChatBox(CreateChatBoxDTO createChatBoxDTO)
        {
            var chatBox = new ChatBox
            {
                ChatBoxID = 0,
                JobID= createChatBoxDTO.JobID,
                IsViewed = true,
                EmployerID = createChatBoxDTO.EmployerID,
                FreelancerID = createChatBoxDTO.FreelancerID
            };
            if(isChatBoxExisted(chatBox)) {
                var a = _context.ChatBoxs.Where(x => x.EmployerID == chatBox.EmployerID &&
                x.FreelancerID == chatBox.FreelancerID && x.JobID == chatBox.JobID).FirstOrDefault();
                return new ChatDTO
                {

                    ChatBoxID = a.ChatBoxID,
                    /* EmployerID = chatBox.EmployerID,
                     FreelancerID = chatBox.FreelancerID*/

                };
            }
            
            _context.ChatBoxs.Add(chatBox);
            _context.SaveChanges();

            return new ChatDTO
            {

                ChatBoxID = chatBox.ChatBoxID,
               /* EmployerID = chatBox.EmployerID,
                FreelancerID = chatBox.FreelancerID*/
                
            };
        }

      
    }
}


