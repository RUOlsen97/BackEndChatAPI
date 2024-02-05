using Blazored.SessionStorage;
using BlazorWasmAuthentication.Models;
using FrontEnd1.Components.Account;
using FrontEnd1.Components.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
//using FrontEnd1.Components.Requirements;
namespace FrontEnd1.Components.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public LoginModel loginModel = new LoginModel();
        private readonly IHttpClientFactory _httpClient;
        private ISessionStorageService _sessionStorageService;
        public const string JWT_Key = nameof(JWT_Key);
        public const string REFRESH_KEY = nameof(REFRESH_KEY);
        bool authenticated {  get; set; }
        bool LoggedIn { get; set; }
        private HttpResponseMessage _loginResponse;

        private string? _jwtCache;

        public event Action<string?>? LoginChange;

        public AuthenticationService(IHttpClientFactory factory, ISessionStorageService sessionStorageService)
        {
            _httpClient = factory;
            _sessionStorageService = sessionStorageService;
        }

        public async ValueTask<string> GetJwtAsync()
        {
            if (string.IsNullOrEmpty(_jwtCache))
            {
                _jwtCache = await _sessionStorageService.GetItemAsync<string>(JWT_Key);
            }
            return _jwtCache;
        }

        public async Task LogoutAsync()
        {
            await _sessionStorageService.RemoveItemAsync(JWT_Key);
            _jwtCache = null;
            LoginChange?.Invoke(null);
        }
        public string GetUsername(string token)
        {
            var jwt = new JwtSecurityToken(token);
            return jwt.Claims.First(c => c.Type == ClaimTypes.Email).Value;
        }
        public async Task<DateTime> LoginAsync(string email, string password)
        {
            
            using var client = _httpClient.CreateClient();

            var uri = new Uri("https://localhost:7222/login");

            var json = $"{{\"email\":\"{email}\",\"password\":\"{password}\", \"twoFactorCode\": \"string\",\"twoFactorRecoveryCode\": \"string\"}}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            _loginResponse = await client.PostAsync(uri, content);

            if (!_loginResponse.IsSuccessStatusCode)
            {
                throw new UnauthorizedAccessException("Login failed");
            }
            bool LoggedIn = true;
            var contentfromresponse = await _loginResponse.Content.ReadAsStringAsync();
            JObject jsonresponse = JObject.Parse(contentfromresponse);
            //if (contentfromresponse == null)
            //{
            //    throw new InvalidDataException();
            //}
            string accesstoken = jsonresponse["accessToken"].ToString();
            //string refreshtoken = jsonresponse["refreshToken"].ToString();
            string expiresIn = jsonresponse["expiresIn"].ToString();

            if (DateTime.TryParse(expiresIn, out DateTime result))
            {
                Console.WriteLine(result);
            }

            await _sessionStorageService.SetItemAsync(JWT_Key, accesstoken);
            //await _sessionStorageService.SetItemAsync(REFRESH_KEY, refreshtoken);
            LoginChange?.Invoke(GetUsername(accesstoken));
            return result;

        }
        public async Task <bool> Authenticate(bool authenticated)
        {
            if (!authenticated)
            {
                // Access the stored response from LoginAsync
                if (_loginResponse != null && _loginResponse.IsSuccessStatusCode)
                {
                    //var contentFromResponse = await _loginResponse.Content.ReadAsStringAsync();
                    //JObject jsonResponse = JObject.Parse(contentFromResponse);
                    //string accessToken = jsonResponse["accessToken"].ToString();
                    //string refreshToken = jsonResponse["refreshToken"].ToString();

                    //// Defer the JavaScript interop calls to OnAfterRenderAsync
                    //_accessTokenToSet = accessToken;
                    //_refreshTokenToSet = refreshToken;

                    //if (_accessTokenToSet != null)
                    //{
                    //    await _sessionStorageService.SetItemAsync(JWT_Key, _accessTokenToSet);
                    //    _accessTokenToSet = null; // Reset the value after setting it
                    //}

                    //if (_refreshTokenToSet != null)
                    //{
                    //    await _sessionStorageService.SetItemAsync(REFRESH_KEY, _refreshTokenToSet);
                    //    _refreshTokenToSet = null; // Reset the value after setting it
                    //}
                    loginModel.LoggedIn = !authenticated;
                    //LoggedInRequirement loggedInRequirement = new LoggedInRequirement(true);
                }
            }
            _jwtCache = _accessTokenToSet;
            return true;
        }

        public string _accessTokenToSet;
        public string _refreshTokenToSet;

        public async Task OnAfterRenderAsync()
        {
            
            // Perform JavaScript interop calls here
            if (_accessTokenToSet != null)
            {
                await _sessionStorageService.SetItemAsync(JWT_Key, _accessTokenToSet);
                _accessTokenToSet = null; // Reset the value after setting it
            }

            if (_refreshTokenToSet != null)
            {
                await _sessionStorageService.SetItemAsync(REFRESH_KEY, _refreshTokenToSet);
                _refreshTokenToSet = null; // Reset the value after setting it
            }
        }
    }
}
