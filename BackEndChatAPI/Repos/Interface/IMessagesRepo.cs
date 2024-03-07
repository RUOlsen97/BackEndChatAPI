using BackEndChatAPI.Models;

namespace BackEndChatAPI.Repos.Interface
{
    public interface IMessagesRepo
    {
        public Messages addMessage(Messages newMessage);
        public List<Messages> GetMessages();
        Task<List<Messages>> GetMessagesByUser(string username);
    }
}