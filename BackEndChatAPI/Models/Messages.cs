namespace BackEndChatAPI.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string User { get; set; }
        public int ChatHubId { get; set; }
    }
}
