
using Infrastructure.ApiotService.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Contracts.SharedDtos;
using Newtonsoft.Json;
using System.Collections.Concurrent;
namespace Infrastructure.ApiotService.Services
{
    public class ApiService<T> : IApiService<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _InstnceKey = new Dictionary<string, string>();

        private static ConcurrentDictionary<string, int> _dailyRequestCount = new ConcurrentDictionary<string, int>();
        // clear 
        private static DateTime _lastRequestDate = DateTime.MinValue;
        private static readonly int MaxRequestsPerDay = 3333;
        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://otapi.net/service-json/");
            _InstnceKey["InstanceKey"] = configuration["ApiConfig:instanceKey"] ?? throw new ArgumentNullException("InstanceKey is missing!");
        }

        // rate limit  to api
        private async Task<bool> IsRequestAllowedAsync()
        {
            var currentDate = DateTime.UtcNow.Date;

            if (_lastRequestDate != currentDate)
            {
                _lastRequestDate = currentDate;
                _dailyRequestCount.Clear();
            }

            var totalRequestsToday = _dailyRequestCount.GetOrAdd(currentDate.ToString("yyyyMMdd"), 0);

            if (totalRequestsToday >= MaxRequestsPerDay)
            {
                return false;
            }

            _dailyRequestCount[currentDate.ToString("yyyyMMdd")] = totalRequestsToday + 1;

            return true;
        }


        public string BuildUrl(string endpoint, Dictionary<string, string> parameters)
        {

            if (parameters == null || parameters.Count == 0)
            {

                return endpoint;

            }

            var quary = string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));

            return endpoint + "?" + quary;


        }

        public async Task<ApiResponse<T>> GetDataAsync(string endpoint, Dictionary<string, string> parameters = null)
        {

            try
            {

                if (!await IsRequestAllowedAsync())
                {
                    throw new Exception("تم تجاوز الحد الأقصى للطلبات اليوم.");
                }

                if (!parameters.ContainsKey("instanceKey"))
                    parameters.Add("instanceKey", _InstnceKey["InstanceKey"]);
                var url = BuildUrl(endpoint, parameters);
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>();

                }
                var json = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<ApiResponse<T>>(json);
            }
            catch
            {

                throw new Exception("Api Not Work");

            }
        }

        public async Task<dynamic> GetDataAsyncDynmic(string endpoint, Dictionary<string, string> parameters = null)
        {
            try
            {


                if (!await IsRequestAllowedAsync())
                {
                    throw new Exception("Rate Limit To Api");
                }

                parameters ??= new Dictionary<string, string>();

                if (!parameters.ContainsKey("instanceKey"))
                    parameters.Add("instanceKey", _InstnceKey["InstanceKey"]);

                var url = BuildUrl(endpoint, parameters);

                var response = await _httpClient.GetAsync(url);
                Console.WriteLine($"Protocol used: {response.Version}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
                    return new { Error = "Request Failed", StatusCode = response.StatusCode };
                }

                var json = await response.Content.ReadAsStringAsync();

                var jsonSettings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json, jsonSettings);

                if (result != null && result?.ErrorCode != null)
                {
                    Console.WriteLine($"Error {result?.ErrorCode}");
                }
                else
                {
                    Console.WriteLine("ErrorCode not found.");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new { Error = "Api Not Work", ExceptionMessage = ex.Message };
            }
        }




    }
}
