using Microsoft.AspNetCore.Mvc;
using Vale.Geographic.Domain.Enumerable;

namespace Vale.Geographic.Api.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(params RoleEnum[] roles) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[]
            {
                roles
            };
        }
    }
}