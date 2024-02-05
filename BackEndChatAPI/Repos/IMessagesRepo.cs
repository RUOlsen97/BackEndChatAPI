using BackEndChatAPI.Models;

namespace BackEndChatAPI.Repos
{
    public interface IMessagesRepo
    {
        public Messages addMessage(Messages newMessage);
        public List<Messages> GetMessages();
    }
}