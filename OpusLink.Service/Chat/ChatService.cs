using Microsoft.EntityFrameworkCore;
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
        List<ChatBox> getAllChatBox(int userId, int role);
        ChatBox getChatBoxById(int id);
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
                FromEmployer = false,
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
        public ChatDTO CreateChatBox(CreateChatBoxDTO createChatBoxDTO)
        {
            var chatBox = new ChatBox
            {
                EmployerID = createChatBoxDTO.EmployerID,
                FreelancerID = createChatBoxDTO.FreelancerID
            };

            _context.ChatBoxs.Add(chatBox);
            _context.SaveChanges();

            return new ChatDTO
            {
                ChatBoxID = chatBox.ChatBoxID,
                EmployerID = chatBox.EmployerID,
                FreelancerID = chatBox.FreelancerID
            };
        }
    }
}


