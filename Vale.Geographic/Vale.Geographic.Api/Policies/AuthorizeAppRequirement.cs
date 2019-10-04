using Microsoft.AspNetCore.Authorization;

namespace Vale.Geographic.Api.Policies
{
    public class AuthorizeAppRequirement : IAuthorizationRequirement
    {
        public AuthorizeAppRequirement(string token, string environment)
        {
            Token = token;

            Environment = environment;
        }

        public string Token { get; }
        public string Environment { get; }
    }
}