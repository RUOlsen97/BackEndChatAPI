//using FrontEnd1.Components.Models;
//using Microsoft.AspNetCore.Authorization;
//using FrontEnd1.Components.CustAttr;
//using System.Globalization;
//using System.Security.Claims;

//namespace FrontEnd1.Components.Handlers
//{
//    public class LoggedInAuthorizationHandler : AuthorizationHandler<LoggedInAuthorizeAttribute>
//        {
//        public LoginModel loginModel {  get; set; }
//        private readonly ILogger<LoggedInAuthorizationHandler> _logger;

//        public LoggedInAuthorizationHandler(ILogger<LoggedInAuthorizationHandler> logger) 
//        {
//            _logger = logger;
//        }
//        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
//                                                        LoggedInAuthorizeAttribute requirement)
//        {
//            _logger.LogWarning("Evaluating requirement for log in", requirement._loggedIn);

//            context.User.Claims
//            bool loggedin = requirement._loggedIn;
//            if (loggedin == null) 
//            {
//                if (loggedin)
//                {
//                    _logger.LogInformation("User is logged in", requirement._loggedIn);
//                    context.Succeed(requirement);
//                }
//                else
//                {
//                    _logger.LogInformation("User is not logged in");
//                }
//            }
//            else
//            {
//                _logger.LogInformation("Not logged in");
//            }
//            return Task.CompletedTask;
//        }
//    }
//}
