using BackEndChatAPI.context;
using BackEndChatAPI.Models;

namespace BackEndChatAPI.Repos
{
    public class MessagesRepo:IMessagesRepo
    {
        private NewContext _context;
        public MessagesRepo(NewContext context)
        {
            _context = context;
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
    }
}
