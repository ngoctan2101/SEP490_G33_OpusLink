using Microsoft.AspNetCore.SignalR;
using OpusLink.Entity.Models;
using OpusLink.Entity;
using Microsoft.EntityFrameworkCore;
using OpusLink.Entity.DTO;
using AutoMapper;
using OpusLink.Service.Chat;

namespace OpusLink.API.Hubs
{
    public class ChatHub : Hub
    {

		public async Task SendMessage(int chatBoxId, bool fromEmployer, string messageContent)
		{
			IChatService _chatService = new ChatService(new OpusLinkDBContext());
			CreateMessageDTO createMessageDTO = new CreateMessageDTO
			{
				ChatBoxID = chatBoxId,
				FromEmployer = fromEmployer,
				MessageContent = messageContent
			};

			// Create the message
			var createdMessage = _chatService.CreateMessage(createMessageDTO);




			await Clients.Group(chatBoxId.ToString()).SendAsync("ReceiveMessage", fromEmployer, messageContent);


			

			
			

        }

		public async Task JoinChatRoom(int chatBoxId)
		{
			// Thêm người dùng hiện tại vào nhóm có tên là chatBoxId
			await Groups.AddToGroupAsync(Context.ConnectionId, chatBoxId.ToString());
		}
	}
}
