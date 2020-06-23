using Vale.Geographic.Domain.Repositories.Interfaces;
using Vale.Geographic.Domain.Entities.Notification;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Vale.Geographic.Infra.Data.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        public string PushNotificationEndpoint { get; set; }

        private readonly IJWTRepository _jwtRepository;

        public NotificationRepository(IConfiguration configuration, IJWTRepository jwtRepository)
        {
            var pushNotification = configuration.GetSection("PushNotification");

            this.PushNotificationEndpoint = pushNotification["Url"];
            _jwtRepository = jwtRepository;
        }


        public async Task InstalationDevice(string applicationId, string installationId, DeviceUpdate deviceUpdate)
        {
            var token = await _jwtRepository.Auth();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

                var param = new System.Net.Http.StringContent(JsonConvert.SerializeObject(deviceUpdate), Encoding.UTF8, "application/json");


                var response = await client.PutAsync(string.Format("{0}Applications/{1}/Devices/installations/{2}", this.PushNotificationEndpoint, applicationId, installationId), param);

                var responseString = await response.Content.ReadAsStringAsync();


            }
        }

        public async Task<DeviceAdd> RegisterDevice(string applicationId, DeviceAdd device)
        {
            var token = await _jwtRepository.Auth();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                var param = new System.Net.Http.StringContent(JsonConvert.SerializeObject(device), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(string.Format("{0}Applications/{1}/Devices", this.PushNotificationEndpoint, applicationId), param);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<DeviceAdd>(responseString);

            }
        }

        public async Task<NotificationAdd> RegisterNotification(string applicationId, NotificationAdd notificationAddDto)
        {
            var token = await _jwtRepository.Auth();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
                var param = new System.Net.Http.StringContent(JsonConvert.SerializeObject(notificationAddDto), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync(string.Format("{0}Applications/{1}/Notifications", this.PushNotificationEndpoint, applicationId), param);

                var responseString = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<NotificationAdd>(responseString);
            }
        }


    }
}
