namespace FrontEnd1.Components.Models
{

    public class LoginModel
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool? LoggedIn { get; set; }

        //public LoginModel(string? email, string? password)
        //{
        //    Email = email;
        //    Password = password;
        //}
        //public override string ToString()
        //{
        //    string result = $"{{\"email\":\"{Email}\",\"password\":\"{Password}\", \"twoFactorCode\": \"string\",\"twoFactorRecoveryCode\": \"string\"}}";
        //    return result;
        //}
    }
}


