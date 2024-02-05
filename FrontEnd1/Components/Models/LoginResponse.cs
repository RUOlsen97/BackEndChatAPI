namespace FrontEnd1.Components.Models
{
    public class LoginResponse
    {
        public required string tokenType {  get; set; }
        public required string accessToken {  get; set; }
        public DateTime expiresIn {  get; set; }
        public required string refreshToken { get; set; }
    }
}
