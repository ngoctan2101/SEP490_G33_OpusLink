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
        ChatBox getChatBox(int id);

        public class ChatService : IChatService
        {
            private readonly OpusLinkDBContext _context;
            public ChatService (OpusLinkDBContext context)
            {
                context = _context;
            }
            public List<ChatBox> getAllChatBox()
            {
                try
                {
                    var chatBox = _context.ChatBoxs.ToList();
                    return chatBox;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            public ChatBox getChatBox(int id)
            {
                try
                {
                    var chatBox = _context.ChatBoxs.FirstOrDefault(x => x.ChatBoxID == id);
                    return chatBox;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
