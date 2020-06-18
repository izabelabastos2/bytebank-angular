using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class JWTRepository : IJWTRepository
    {
        private readonly IMemoryCache _cache;

        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string ServiceLogin { get; set; }
        public string ServicePass { get; set; }

        public JWTRepository(IConfiguration configuration, IMemoryCache cache)
        {
            var JwtIssuerOptions = configuration.GetSection("JwtIssuerOptions");

            this.Issuer = JwtIssuerOptions["Issuer"];
            this.Audience = JwtIssuerOptions["Audience"];
            this.SecretKey = JwtIssuerOptions["SecretKey"];
            this.ServiceLogin = JwtIssuerOptions["ServiceLogin"];
            this.ServicePass = JwtIssuerOptions["ServicePass"];
            this._cache = cache;
        }

        private async Task<dynamic> AuthRequest()
        {
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "username", this.ServiceLogin},
                    { "password", this.ServicePass},
                    { "grant_type", "password"},
                    { "client_id", this.Audience },
                    { "scope", "notify_api" }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(this.Issuer + "v2/.auth/token", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    // var ret = JsonConvert.DeserializeObject<dynamic>(responseString);

                    return responseString;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Token> Auth()
        {
            string cacheEntry;
            if (!_cache.TryGetValue(this.ServiceLogin, out cacheEntry))
            {
                cacheEntry = await this.AuthRequest();

                if (cacheEntry == null)
                {
                    return null;
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                 .SetSize(1)
                    .SetPriority(CacheItemPriority.High)
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(60));


                _cache.Set(this.ServiceLogin, cacheEntry, cacheEntryOptions);
            }
            return JsonConvert.DeserializeObject<Token>(cacheEntry);



        }
    }
}
