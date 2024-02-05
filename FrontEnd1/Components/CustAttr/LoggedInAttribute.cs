//using Microsoft.AspNetCore.Authorization;
//using FrontEnd1.Components.Models;

//namespace FrontEnd1.Components.CustAttr
//{
//    public class LoggedInAuthorizeAttribute : AuthorizeAttribute, IAuthorizationRequirement, IAuthorizationRequirementData 
//    {
//        internal bool _loggedIn;
//        public LoggedInAuthorizeAttribute(bool LoggedIn) => _loggedIn = LoggedIn;
//        public IEnumerable<IAuthorizationRequirement> GetRequirements()
//        {
//            yield return this;
//        }
//    }
//}
