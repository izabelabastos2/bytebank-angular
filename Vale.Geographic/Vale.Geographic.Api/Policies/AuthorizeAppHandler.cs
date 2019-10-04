using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vale.Geographic.Api.Policies
{
    public class AuthorizeAppHandler : AuthorizationHandler<AuthorizeAppRequirement>
    {
        private const string AuthorizeHeader = "Authorization";

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AuthorizeAppRequirement requirement)
        {
            if (!Startup.AuthenticatedEnvironments.Contains(requirement.Environment.ToLower()))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var token = ((ActionContext)context.Resource).HttpContext.Request.Headers[AuthorizeHeader]
                .FirstOrDefault();

            if (token != null && token.Equals(requirement.Token))
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }
}