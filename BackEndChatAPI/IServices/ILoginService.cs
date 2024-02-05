namespace BackEndChatAPI.IServices
{
    public interface ILoginService
    {
        public bool authenticateUser(string username, string password);
    }
}
