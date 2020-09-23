using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Vale.Geographic.Api.Models.Auth;

namespace Vale.Geographic.Api.Provider
{
    public class IAMAuthorizeAttribute : TypeFilterAttribute
    {
        public IAMAuthorizeAttribute() : base(typeof(IAMAuthorizeFilter))
        {
        }
    }

    public class IAMAuthorizeFilter : IAuthorizationFilter
    {
        private readonly string IDS_DEV = "https://ids-qa.valeglobal.net/nidp/oauth/nam/userinfo";
        private readonly string IDS_QA = "https://ids-qa.valeglobal.net/nidp/oauth/nam/userinfo";
        private readonly string IDS_PRD = "https://ids-prd.valeglobal.net/nidp/oauth/nam/userinfo";

        public IAMAuthorizeFilter()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var access_token = context.HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()
                    .Replace("bearer ", "")
                    .Replace("Bearer ", "");

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower();
                UserInfo userInfo;

                switch (environment)
                {
                    case "production":
                        userInfo = GetUserInfo(IDS_PRD, access_token).Result;
                        break;
                    case "qa":
                        userInfo = GetUserInfo(IDS_QA, access_token).Result;
                        break;
                    case "development":
                    case "dev":
                    case "local":
                        userInfo = GetUserInfo(IDS_DEV, access_token).Result;
                        break;
                    default:
                        userInfo = null;
                        break;
                }

                if (userInfo == null)
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    context.HttpContext.Session.SetString("USER_INFO_IAM_ID", userInfo.EmployeeID);
                }
            } 
            catch (Exception e)
            {
                context.Result = new UnauthorizedResult();
                throw e;
            }
        }

        private async Task<UserInfo> GetUserInfo(string userInfoUrl, string access_token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"bearer {access_token}");

                var response = await client.GetAsync(userInfoUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<UserInfo>(content);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
