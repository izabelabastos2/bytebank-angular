using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Api.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly RoleEnum[] _roles;

        public ClaimRequirementFilter(RoleEnum[] roles)
        {
            this._roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.FilterDescriptors.Any(o => o.Filter.ToString().Contains(nameof(Microsoft.AspNetCore.Mvc.Authorization.AllowAnonymousFilter))))
                return;

            var claims = context.HttpContext.User.Claims.Where(c =>
                c.Type == "groupMembership" &&
                _roles.Any(r => c.Value.ToLower().StartsWith("cn=" + r.ToString().ToLower()))
                || c.Type == "groupMembership" &&
                c.Value.ToLower().StartsWith("cn=" + RoleEnum.Administrator.ToString().ToLower())
            );
            if (!claims.Any())
            {
                context.Result = new ForbidResult();
            }
            else
            {
                if (!claims.Any(c => c.Value.ToLower().StartsWith("cn=" + RoleEnum.Administrator.ToString().ToLower()))
                    && !claims.Any(c => c.Value.ToLower().StartsWith("cn=" + RoleEnum.User.ToString().ToLower())))
                    foreach (var item in claims)
                    {
                        var role = item.Value.Split(',')[0];
                        if (role.ToLower() != "cn=" + RoleEnum.Administrator.ToString().ToLower() &&
                            role.ToLower() != "cn=" + RoleEnum.User.ToString().ToLower())
                            context.HttpContext.Items.Add("center", role.Split('_').Last());
                    }
            }
        }
    }
}