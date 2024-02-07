using BackEndChatAPI.context;
using BackEndChatAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Data.Entity;

namespace BackEndChatAPI.Repos
{
    public class MessagesRepo:IMessagesRepo
    {
        private NewContext _context;
        private readonly UserManager<Users> _userManager;

        public MessagesRepo(NewContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Messages addMessage(Messages newMessage)
        {
            _context.Messages.Add(newMessage);
            _context.SaveChanges();
            return newMessage;
        }
        public List<Messages> GetMessages()
        {
            return _context.Set<Messages>().ToList();
        }
        public async Task <List<Messages>> GetMessagesByUser(string id)
        {
            var user1 = await _userManager.FindByIdAsync(id);
            var messagesList = _context.Messages
                .Where(user => user.User == user1.UserName).ToList();

            return messagesList;
        }
    }
}
