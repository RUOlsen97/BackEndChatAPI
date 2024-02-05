//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.Extensions.Options;
//using FrontEnd1.Components.Requirements;

//namespace FrontEnd1.Components.Account
//{
//    public class LoggedInPolicyProvider : IAuthorizationPolicyProvider
//    {
//        const string Policy_prefix = "LoggedIn";
//        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }
//        public LoggedInPolicyProvider(IOptions<AuthorizationOptions> options)
//        {
//            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
//        }
//        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
//                            FallbackPolicyProvider.GetDefaultPolicyAsync();
//        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
//                                FallbackPolicyProvider.GetFallbackPolicyAsync();
//        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
//        {
//            if (policyName.StartsWith(Policy_prefix, StringComparison.OrdinalIgnoreCase) &&
//                bool.TryParse(policyName.Substring(Policy_prefix.Length), out var LoggedIn))
//            {
//                var policy = new AuthorizationPolicyBuilder(
//                                                    JwtBearerDefaults.AuthenticationScheme);
//                policy.AddRequirements(new LoggedInRequirement(LoggedIn));
//                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
//            }

//            return Task.FromResult<AuthorizationPolicy?>(null);
//        }
//    }
//}
