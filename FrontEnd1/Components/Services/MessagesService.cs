using static Duende.IdentityServer.Models.IdentityResources;
using System.Text;
using NuGet.Protocol.Plugins;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using NuGet.Packaging.Signing;
using FrontEnd1.Components.Models;

namespace FrontEnd1.Components.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IHttpClientFactory _httpClient;
        private HttpResponseMessage _MessageResponse;

        List<getMessagesModel> dbmessages { get; set; }

        public MessagesService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendMessage(int id, string text, string user, int chatHubId)
        {
            using var client = _httpClient.CreateClient();
            var uri = new Uri("https://localhost:7222/api/Messages");
            var json = $"{{\"id\":\"{id}\",\"text\":\"{text}\", \"user\": \"{user}\",\"chatHubId\": \"{chatHubId}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _MessageResponse = await client.PostAsync(uri, content);
            if (!_MessageResponse.IsSuccessStatusCode)
            {
                throw new Exception("Couldn't post message to db");
            }
        }
        public async Task<List<getMessagesModel>> GetMessages()
        {

            using var client = _httpClient.CreateClient();
            dbmessages = await client.GetFromJsonAsync<List<getMessagesModel>>("https://localhost:7222/api/Messages");

            return dbmessages;
        }
    }
}
