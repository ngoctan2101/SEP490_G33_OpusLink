using Microsoft.EntityFrameworkCore;
using OpusLink.Entity;
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
        ChatBox getChatBoxById(int id);
        List<Message>  GetMessageById(int id);
    }
        public class ChatService : IChatService
        {
            private readonly OpusLinkDBContext _context;
            public ChatService (OpusLinkDBContext context)
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

        public List<Message>  GetMessageById(int id)
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

        
    }
    }
    

