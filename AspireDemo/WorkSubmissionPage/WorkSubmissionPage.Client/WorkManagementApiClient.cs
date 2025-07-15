using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace WorkSubmissionPage.Client
{
    public class WorkManagementApiClient
    {
        private const string WorkUnitRelativeUrl = "/WorkUnits/";

        private readonly HttpClient _client;

        public WorkManagementApiClient(HttpClient client)
        {
            _client = client;
        }

        internal async void PostWork(WorkUnitPostDto workUnitDto)
        {
            // Serialize the request object to JSON
            var jsonBody = JsonSerializer.Serialize(workUnitDto);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await _client.PostAsync(WorkUnitRelativeUrl, content);
        }

        internal async Task<IEnumerable<WorkResult>> GetWorkResults()
        {
            // Send the POST request
            var response = await _client.GetAsync(WorkUnitRelativeUrl);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Read the response body as a string
            var responseBody = response.Content.ReadAsStream();

            // Deserialize the response JSON into a MyResponse object
            var responseObject = JsonSerializer.Deserialize<WorkResult[]>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return responseObject.Reverse();
        }

        internal static WorkManagementApiClient CreateClient(string baseAddress)
        {
            HttpClient httpClinet = new HttpClient() { BaseAddress = new Uri(baseAddress) };
            return new WorkManagementApiClient(httpClinet);
        }
    }
}
