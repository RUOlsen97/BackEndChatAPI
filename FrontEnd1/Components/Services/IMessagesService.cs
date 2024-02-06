

using FrontEnd1.Components.Models;
using NuGet.Protocol.Plugins;

namespace FrontEnd1.Components.Services
{
    public interface IMessagesService
    {
        Task<List<getMessagesModel>> GetMessages();
        Task SendMessage(int id, string text, string user, int chatHubId);
    }
}