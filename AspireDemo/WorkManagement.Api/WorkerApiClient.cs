using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace WorkManagement.Api;

public class WorkerApiClient
{
    private readonly HttpClient _client;

    public WorkerApiClient(HttpClient client)
    {
        _client = client;
    }

    internal WorkResult GetWorkResult(WorkUnitPostDto workUnitDto)
    {
        // perform work
        var workerUrl = "/Worker/";

        // Serialize the request object to JSON
        var jsonBody = JsonSerializer.Serialize(workUnitDto);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        // Send the POST request
        var postTask = _client.PostAsync(workerUrl, content);
        postTask.Wait();

        var response = postTask.Result;

        // Ensure the response is successful
        response.EnsureSuccessStatusCode();

        // Read the response body as a string
        var responseBody = response.Content.ReadAsStream();

        // Deserialize the response JSON into a MyResponse object
        var responseObject = JsonSerializer.Deserialize<WorkResult>(responseBody, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        return responseObject;
    }
}