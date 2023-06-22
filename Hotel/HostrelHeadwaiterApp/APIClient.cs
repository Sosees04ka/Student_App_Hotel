using HotelContracts.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HostrelHeadwaiterApp
{
    public class APIClient
    {
        private static readonly HttpClient _headwaiter = new();

        public static HeadwaiterViewModel? Headwaiter { get; set; } = null;

        public static void Connect(IConfiguration configuration)
        {
            _headwaiter.BaseAddress = new Uri(configuration["IPAddress"]);
            _headwaiter.DefaultRequestHeaders.Accept.Clear();
            _headwaiter.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static T? GetRequest<T>(string requestUrl)
        {
            var response = _headwaiter.GetAsync(requestUrl);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception(result);
            }
        }

        public static void PostRequest<T>(string requestUrl, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _headwaiter.PostAsync(requestUrl, data);

            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (!response.Result.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
        }
    }
}
